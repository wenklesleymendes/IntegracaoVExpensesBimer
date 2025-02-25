﻿namespace PARS.Inhouse.Systems.Shared.DTOs.Request.Bimer
{
    public class BimerRequestDto
    {
        public string? dataCadastro { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public List<Item>? Itens { get; set; }
        public int Valor { get; set; }
        public string? IdentificadorBanco { get; set; }
        public string? IdentificadorCategoriaPessoa { get; set; }
        public string? IdentificadorFormaPagamento { get; set; }
        public string? IdentificadorModalidadePagamento { get; set; }
        public string? CodigoEmpresa { get; set; }
        public string? DataEmissao { get; set; }
        public string? DataReferencia { get; set; }
        public string? DataVencimento { get; set; }
        public string? Descricao { get; set; }
        public DesmembramentoCOFINS? DesmembramentoCOFINS { get; set; }
        public DesmembramentoCSLL? DesmembramentoCSLL { get; set; }
        public DesmembramentoDesconto? DesmembramentoDesconto { get; set; }
        public DesmembramentoINSS? DesmembramentoINSS { get; set; }
        public DesmembramentoIRRF? DesmembramentoIRRF { get; set; }
        public DesmembramentoISS? DesmembramentoISS { get; set; }
        public DesmembramentoJuros? DesmembramentoJuros { get; set; }
        public DesmembramentoMulta? DesmembramentoMulta { get; set; }
        public DesmembramentoOutros? DesmembramentoOutros { get; set; }
        public DesmembramentoPIS? DesmembramentoPIS { get; set; }
        public DesmembramentoPisCofinsCsll? DesmembramentoPisCofinsCsll { get; set; }
        public string IdentificadorPessoa { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string NumeroAgenciaBancaria { get; set; } = string.Empty;
        public string NumeroCodigoBarra { get; set; } = string.Empty;
        public string NumeroContaBancaria { get; set; } = string.Empty;
        public string NumeroTituloBanco { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public bool Previsao { get; set; }
        public string TipoLiquidacao { get; set; } = string.Empty;
    }

    public class Item
    {
        public string IdentificadorNaturezaLancamento { get; set; } = string.Empty;
        public List<CentroCusto>? CentroDeCusto { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public int Valor { get; set; }
    }

    public class CentroCusto
    {
        public int AliquotaPorcentagem { get; set; }
        public string DataLancamento { get; set; } = string.Empty;
        public string IdentificadorCentroDeCusto { get; set; } = string.Empty;
        public int ValorLancamento { get; set; }
    }

    public class DesmembramentoCOFINS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoCSLL
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoDesconto
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoINSS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoIRRF
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoISS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoJuros
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoMulta
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoOutros
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoPIS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    public class DesmembramentoPisCofinsCsll
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }
}