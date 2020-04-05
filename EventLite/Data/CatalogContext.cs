﻿using EventLite.Domain.EventLite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogApi.Data
{
    public class CatalogContext : DbContext
    {
        // Where
        public CatalogContext(DbContextOptions options) : base(options) { }



        // What
        public DbSet<CatalogEvent> CatalogEvent { get; set; }

        public DbSet<CatalogFormat> CatalogFormats { get; set; }

        public DbSet<CatalogTopic> CatalogTopics { get; set; }

        // How
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // QUESTION:
            // Want to know how to use the Type type overload of Entity


            modelBuilder.Entity<CatalogEvent>(entity =>
                {
                    entity.ToTable("CatalogEvents");

                    entity.Property(e => e.Id)
                        .IsRequired()
                        .UseHiLo("catalog_events_hilo");

                    entity.Property(e => e.Title)
                        .HasMaxLength(100)
                        .IsRequired();

                    entity.Property(e => e.Description)
                        .HasMaxLength(1000);

                    // TODO: What happens with the DateTime type?
                    entity.Property(e => e.Start)
                        .HasMaxLength(100)
                        .IsRequired();

                    entity.Property(e => e.End)
                        .HasMaxLength(100);


                    // No config for PictureUrl...what is the default?
                    
                    // TODO: What is the right way to do this composition
                    entity.Property(e => e.Venue.Name)
                          .HasMaxLength(100)
                          .IsRequired();
                    entity.Property(e => e.Venue.AddressLine1)
                          .HasMaxLength(100)
                          .IsRequired();
                    entity.Property(e => e.Venue.AddressLine2)
                          .HasMaxLength(100);
                    entity.Property(e => e.Venue.AddressLine3)
                          .HasMaxLength(100);
                    entity.Property(e => e.Venue.City)
                          .HasMaxLength(100)
                          .IsRequired();
                    entity.Property(e => e.Venue.StateProvince)
                          .HasMaxLength(2)
                          .IsFixedLength()
                          .IsRequired();
                    entity.Property(e => e.Venue.PostalCode)
                          .HasMaxLength(5)
                          .IsFixedLength()
                          .IsRequired();


                    entity.Property(e => e.HostOrganizer)
                          .HasMaxLength(100)
                          .IsRequired();


                    entity.HasOne(e => e.CatalogFormat)
                         // TODO: Figure out hypothetical for why doesn't work (see: CatalogFormat)
                         //.WithMany(f => f.CatalogEvent)
                         .WithMany()
                         .HasForeignKey(e => e.CatalogFormatId);

                    entity.HasOne(e => e.CatalogTopic)
                        .WithMany()
                        .HasForeignKey(e => e.CatalogTopicId);

                }
                            
             );



            modelBuilder.Entity<CatalogFormat>(entity =>
            {
                entity.ToTable("CatalogFormats");

                entity.Property(f => f.Id)
                    .IsRequired()
                    .UseHiLo("catalog_formats_hilo");

                entity.Property(f => f.Format)
                    .IsRequired()
                    .HasMaxLength(100);
                
            
            });


            modelBuilder.Entity<CatalogTopic>(entity =>
            {
                entity.ToTable("CatalogTopics");

                entity.Property(f => f.Id)
                    .IsRequired()
                    .UseHiLo("catalog_topics_hilo");

                entity.Property(f => f.Topic)
                    .IsRequired()
                    .HasMaxLength(100);


            });
        }

    }

}
