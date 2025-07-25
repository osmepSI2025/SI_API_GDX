﻿using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class MApiInformation
{
    public int Id { get; set; }

    public string? ServiceNameTh { get; set; }

    public string? ServiceNameCode { get; set; }

    public string? Urlproduction { get; set; }

    public string? Urldevelopment { get; set; }

    public string? AuthorizationType { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? ApiKey { get; set; }

    public string? Bearer { get; set; }

    public string? AccessToken { get; set; }

    public string? ContentType { get; set; }

    public string? MethodType { get; set; }

    public string? ConsumerKey { get; set; }

    public string? ConsumerSecret { get; set; }

    public string? Token { get; set; }

    public string? AgentId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
