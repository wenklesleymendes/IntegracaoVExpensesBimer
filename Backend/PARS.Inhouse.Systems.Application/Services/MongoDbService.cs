using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Shared.DTOs.TemplatesPlanilha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService()
        {
            var connectionUri = "mongodb+srv://raphaelmello2806:QyrVmxSadCw9grBl@cluster0.mr2va.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            var client = new MongoClient(settings);
            _database = client.GetDatabase("PARSINHOUSE");
        }

        public async Task CriarOuAtualizarSchemaDinamicoAsync(string nomeTemplate, List<MapeamentoTemplatePlanilhaDto> mappings)
        {
            string collectionName = $"Template_{nomeTemplate}";
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            // Verifica se a coleção já existe
            var collections = await _database.ListCollectionNames().ToListAsync();
            bool collectionExists = collections.Contains(collectionName);

            // Criar um HashSet com os campos esperados do DeParaMappings
            var expectedFields = new HashSet<string>();
            foreach (var mapping in mappings)
            {
                expectedFields.Add(mapping.ColunaPlanilha);
            }

            if (!collectionExists)
            {
                Console.WriteLine($"Criando nova coleção '{collectionName}'...");

                // Criar um novo schema baseado no DeParaMappings
                var novoSchema = new BsonDocument();
                foreach (var field in expectedFields)
                {
                    novoSchema[field] = BsonValue.Create("");
                }

                await collection.InsertOneAsync(novoSchema);
                Console.WriteLine($"Coleção '{collectionName}' criada com sucesso.");
            }
            else
            {
                Console.WriteLine($"Atualizando schema da coleção '{collectionName}'...");

                // Busca o primeiro documento como referência
                var existingSchema = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

                if (existingSchema != null)
                {
                    var updatedSchema = new BsonDocument();

                    // Adiciona apenas os campos que estão no DeParaMappings
                    foreach (var field in expectedFields)
                    {
                        updatedSchema[field] = existingSchema.Contains(field) ? existingSchema[field] : BsonValue.Create("");
                    }

                    // Atualiza o documento com os novos campos e remove os que não estão mais no DeParaMappings
                    await collection.ReplaceOneAsync(new BsonDocument(), updatedSchema);

                    Console.WriteLine($"Schema da coleção '{collectionName}' atualizado (novos campos adicionados e antigos removidos).");
                }
                else
                {
                    Console.WriteLine("Nenhum documento encontrado na coleção existente. Criando um novo schema...");

                    var novoSchema = new BsonDocument();
                    foreach (var field in expectedFields)
                    {
                        novoSchema[field] = BsonValue.Create("");
                    }

                    await collection.InsertOneAsync(novoSchema);
                    Console.WriteLine($"Novo documento de schema inserido na coleção '{collectionName}'.");
                }
            }
        }
        public async Task<List<string>> ListarTodasColecoesAsync()
        {
            try
            {
                // Obtém todas as coleções do banco de dados
                var collections = await _database.ListCollectionNames().ToListAsync();

                // Filtra as coleções que começam com "Template_" (se necessário)
                var templateCollections = collections.Where(c => c.StartsWith("Template_")).ToList();

                return templateCollections;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar as coleções: {ex.Message}");
                return new List<string>(); // Retorna uma lista vazia em caso de erro
            }
        }

        public async Task<IActionResult> RemoverColecaoAsync(string nomeTemplate)
        {
            try
            {
                string collectionName = $"Template_{nomeTemplate}";

                // Verifica se a coleção existe
                var collections = await _database.ListCollectionNames().ToListAsync();
                if (!collections.Contains(collectionName))
                {
                    return null;
                }

                // Remove a coleção
                await _database.DropCollectionAsync(collectionName);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover a coleção: {ex.Message}");
                return null;
            }
        }
        public async Task<TemplatePlanilhaDto> BuscarCamposPorNomeTemplateAsync(string nomeTemplate)
        {
            try
            {
                string collectionName = $"{nomeTemplate}";
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                // Busca o primeiro documento da coleção para obter os campos (chaves)
                var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

                if (document == null)
                {
                    Console.WriteLine($"Nenhum documento encontrado na coleção '{collectionName}'.");
                    return null;
                }

                // Extrai os campos (chaves) do documento encontrado
                var campos = document.Names.Where(name => name != "_id").ToList(); // Ignorar o _id

                var dto = new TemplatePlanilhaDto()
                {
                    NomeTemplate = nomeTemplate,
                    DeParaMappings = campos.Select((c, index) => new MapeamentoTemplatePlanilhaDto { ColunaPlanilha = c, LetraColunaPlanilha = GetColumnLetter(index) }).ToList()
                };
                return dto; // Retorna uma lista com os campos para edição
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetColumnLetter(int index)
        {
            int letterIndex = index;
            string columnLetter = "";

            while (letterIndex >= 0)
            {
                columnLetter = (char)('A' + (letterIndex % 26)) + columnLetter;
                letterIndex = letterIndex / 26 - 1;
            }

            return columnLetter;
        }
    }
}
