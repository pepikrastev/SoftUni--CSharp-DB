﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }
        //2.	Hospital Database Modification
        public DbSet<Doctor> Doctors { get; set; }
        // 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);

                entity.HasMany(p => p.Visitations).WithOne(v => v.Patient);
                entity.HasMany(p => p.Diagnoses).WithOne(d => d.Patient);

                entity.Property(p => p.FirstName).HasMaxLength(50).IsRequired().IsUnicode();
                entity.Property(p => p.LastName).HasMaxLength(50).IsRequired().IsUnicode();
                entity.Property(p => p.Address).HasMaxLength(250).IsRequired().IsUnicode();
                entity.Property(p => p.Email).HasMaxLength(80).IsRequired().IsUnicode();
                entity.Property(p => p.HasInsurance).IsRequired();

            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasKey(v => v.VisitationId);

                entity.Property(v => v.Comments).HasMaxLength(250).IsUnicode();
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(d => d.DiagnoseId);

                entity.Property(d => d.Name).HasMaxLength(50).IsRequired().IsUnicode();
                entity.Property(d => d.Comments).HasMaxLength(250).IsUnicode();
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.MedicamentId);

                entity.Property(m => m.Name).HasMaxLength(50).IsRequired().IsUnicode();
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new
                {
                    pm.PatientId,
                    pm.MedicamentId
                });

                entity.HasOne(pm => pm.Patient).WithMany(p => p.Prescriptions).HasForeignKey(pm => pm.PatientId);

                entity.HasOne(pm => pm.Medicament).WithMany(m => m.Prescriptions).HasForeignKey(pm => pm.MedicamentId);
            });

           // 2.Hospital Database Modification
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId);

               // entity.Property(d => d.Name).HasMaxLength(50).IsRequired().IsUnicode();
               // entity.Property(d => d.Specialty).HasMaxLength(50).IsRequired().IsUnicode();

                entity.HasMany(d => d.Visitations).WithOne(v => v.Doctor);
            });
        }
    }
}
