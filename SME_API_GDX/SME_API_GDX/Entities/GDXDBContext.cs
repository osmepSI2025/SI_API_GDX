using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SME_API_GDX.Entities;

public partial class GDXDBContext : DbContext
{
    public GDXDBContext()
    {
    }

    public GDXDBContext(DbContextOptions<GDXDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MApiInformation> MApiInformations { get; set; }

    public virtual DbSet<MOrganizationJuristicPerson> MOrganizationJuristicPeople { get; set; }

    public virtual DbSet<MScheduledJob> MScheduledJobs { get; set; }

    public virtual DbSet<TJuristicObjective> TJuristicObjectives { get; set; }

    public virtual DbSet<TJuristicPersonList> TJuristicPersonLists { get; set; }

    public virtual DbSet<TOrganizationJuristicAddress> TOrganizationJuristicAddresses { get; set; }

    public virtual DbSet<TOrganizationJuristicPersonDescription> TOrganizationJuristicPersonDescriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_CI_AS");

        modelBuilder.Entity<MApiInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MApiInformation");

            entity.ToTable("M_ApiInformation", "SME_GDX");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessToken).HasColumnName("accessToken");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MOrganizationJuristicPerson>(entity =>
        {
            entity.HasKey(e => e.OrganizationJuristicId).HasName("PK__M_Organi__6E20652EF94B9040");

            entity.ToTable("M_OrganizationJuristicPerson", "SME_GDX");

            entity.Property(e => e.OrganizationJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationJuristicID");
            entity.Property(e => e.DigitalIdflag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DigitalIDFlag");
            entity.Property(e => e.OrganizationJuristicBranchName).HasMaxLength(255);
            entity.Property(e => e.OrganizationJuristicNameEn)
                .HasMaxLength(255)
                .HasColumnName("OrganizationJuristicNameEN");
            entity.Property(e => e.OrganizationJuristicNameTh)
                .HasMaxLength(255)
                .HasColumnName("OrganizationJuristicNameTH");
            entity.Property(e => e.OrganizationJuristicPaidUpCapital).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrganizationJuristicRegisterCapital).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrganizationJuristicRegisterDate).HasColumnType("date");
            entity.Property(e => e.OrganizationJuristicStatus).HasMaxLength(100);
            entity.Property(e => e.OrganizationJuristicType).HasMaxLength(100);
            entity.Property(e => e.OrganizationOldJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationOldJuristicID");
        });

        modelBuilder.Entity<MScheduledJob>(entity =>
        {
            entity.ToTable("M_ScheduledJobs", "SME_GDX");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobName).HasMaxLength(150);
        });

        modelBuilder.Entity<TJuristicObjective>(entity =>
        {
            entity.HasKey(e => e.ObjectiveId).HasName("PK__T_Jurist__8C56338DD89D717F");

            entity.ToTable("T_JuristicObjective", "SME_GDX");

            entity.Property(e => e.ObjectiveId).HasColumnName("ObjectiveID");
            entity.Property(e => e.JuristicObjective)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.JuristicObjectiveCode).HasMaxLength(10);
            entity.Property(e => e.JuristicObjectiveTextEn)
                .HasMaxLength(255)
                .HasColumnName("JuristicObjectiveTextEN");
            entity.Property(e => e.JuristicObjectiveTextTh)
                .HasMaxLength(255)
                .HasColumnName("JuristicObjectiveTextTH");
            entity.Property(e => e.OrganizationJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationJuristicID");

            entity.HasOne(d => d.OrganizationJuristic).WithMany(p => p.TJuristicObjectives)
                .HasForeignKey(d => d.OrganizationJuristicId)
                .HasConstraintName("FK__T_Juristi__Organ__1273C1CD");
        });

        modelBuilder.Entity<TJuristicPersonList>(entity =>
        {
            entity.HasKey(e => e.PersonListId).HasName("PK__T_Jurist__F6A30F1D4D3E0D9C");

            entity.ToTable("T_JuristicPersonList", "SME_GDX");

            entity.Property(e => e.PersonListId).HasColumnName("PersonListID");
            entity.Property(e => e.JuristicPersonInvestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.JuristicPersonInvestType).HasMaxLength(100);
            entity.Property(e => e.JuristicPersonType).HasMaxLength(100);
            entity.Property(e => e.OrganizationJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationJuristicID");
            entity.Property(e => e.PersonFirstNameTh)
                .HasMaxLength(100)
                .HasColumnName("PersonFirstNameTH");
            entity.Property(e => e.PersonLastNameTh)
                .HasMaxLength(100)
                .HasColumnName("PersonLastNameTH");
            entity.Property(e => e.PersonMiddleNameTh)
                .HasMaxLength(100)
                .HasColumnName("PersonMiddleNameTH");
            entity.Property(e => e.PersonNameTitleTextTh)
                .HasMaxLength(50)
                .HasColumnName("PersonNameTitleTextTH");

            entity.HasOne(d => d.OrganizationJuristic).WithMany(p => p.TJuristicPersonLists)
                .HasForeignKey(d => d.OrganizationJuristicId)
                .HasConstraintName("FK__T_Juristi__Organ__15502E78");
        });

        modelBuilder.Entity<TOrganizationJuristicAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__T_Organi__091C2A1BE3E2BE5B");

            entity.ToTable("T_OrganizationJuristicAddress", "SME_GDX");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AddressNo).HasMaxLength(50);
            entity.Property(e => e.Building).HasMaxLength(255);
            entity.Property(e => e.CityCode).HasMaxLength(50);
            entity.Property(e => e.CitySubDivisionCode).HasMaxLength(50);
            entity.Property(e => e.CitySubDivisionTextTh)
                .HasMaxLength(255)
                .HasColumnName("CitySubDivisionTextTH");
            entity.Property(e => e.CityTextTh)
                .HasMaxLength(255)
                .HasColumnName("CityTextTH");
            entity.Property(e => e.CountrySubDivisionCode).HasMaxLength(50);
            entity.Property(e => e.CountrySubDivisionTextTh)
                .HasMaxLength(255)
                .HasColumnName("CountrySubDivisionTextTH");
            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.Moo).HasMaxLength(50);
            entity.Property(e => e.OrganizationJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationJuristicID");
            entity.Property(e => e.Road).HasMaxLength(255);
            entity.Property(e => e.RoomNo).HasMaxLength(50);
            entity.Property(e => e.Soi).HasMaxLength(255);
            entity.Property(e => e.Trok).HasMaxLength(255);
            entity.Property(e => e.Village).HasMaxLength(255);
            entity.Property(e => e.Yaek).HasMaxLength(50);

            entity.HasOne(d => d.OrganizationJuristic).WithMany(p => p.TOrganizationJuristicAddresses)
                .HasForeignKey(d => d.OrganizationJuristicId)
                .HasConstraintName("FK__T_Organiz__Organ__182C9B23");
        });

        modelBuilder.Entity<TOrganizationJuristicPersonDescription>(entity =>
        {
            entity.HasKey(e => e.DescriptionId).HasName("PK__T_Organi__A58A9FEBD14867E9");

            entity.ToTable("T_OrganizationJuristicPersonDescription", "SME_GDX");

            entity.Property(e => e.DescriptionId).HasColumnName("DescriptionID");
            entity.Property(e => e.DescriptionType).HasMaxLength(255);
            entity.Property(e => e.OrganizationJuristicId)
                .HasMaxLength(20)
                .HasColumnName("OrganizationJuristicID");

            entity.HasOne(d => d.OrganizationJuristic).WithMany(p => p.TOrganizationJuristicPersonDescriptions)
                .HasForeignKey(d => d.OrganizationJuristicId)
                .HasConstraintName("FK__T_Organiz__Organ__1B0907CE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
