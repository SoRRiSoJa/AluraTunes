using AluraTunes.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace AluraTunes.Data
{
    class Context:DbContext
    {
        private readonly string _connectionString;

        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            throw new Exception();
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artista> Artistas { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Faixa> Faixas { get; set; }
        public virtual DbSet<Funcionario> Funcionarios { get; set; }
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<ItemNotaFiscal> ItemNotaFiscals { get; set; }
        public virtual DbSet<NotaFiscal> NotaFiscals { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<TipoMidia> TipoMidias { get; set; }
    }
}
