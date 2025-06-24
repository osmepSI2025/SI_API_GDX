using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class TOrganizationJuristicPersonDescription
{
    public int DescriptionId { get; set; }

    public string? OrganizationJuristicId { get; set; }

    public int? DescriptionSequence { get; set; }

    public string? DescriptionType { get; set; }

    public string? DescriptionDetail { get; set; }

    public virtual MOrganizationJuristicPerson? OrganizationJuristic { get; set; }
}
