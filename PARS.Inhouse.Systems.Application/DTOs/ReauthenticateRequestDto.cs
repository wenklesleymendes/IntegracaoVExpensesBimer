﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.DTOs
{
    public class ReauthenticateRequestDto
    {
        public string client_id { get; set; }
        public string grant_type { get; set; } = "refresh_token";
        public string refresh_token { get; set; }
    }
}
