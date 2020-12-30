using AluraTunes.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace AluraTunes.Data
{
    class Context:DbContext
    {
        private readonly string  strConn;

        public Context(string strConn)
        {
            this.strConn = strConn;
        }
        public Context()
        {
            this.strConn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\joao.silva\Documents\Alura\AluraTunes\AluraTunes\Data\AluraTunes.mdf;Integrated Security=True";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(strConn);
        }
        

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Artista> Artista { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Faixa> Faixa { get; set; }
        public virtual DbSet<Funcionario> Funcionario { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<ItemNotaFiscal> ItemNotaFiscal { get; set; }
        public virtual DbSet<NotaFiscal> NotaFiscal { get; set; }
        public virtual DbSet<Playlist> Playlist { get; set; }
        public virtual DbSet<TipoMidia> TipoMidia { get; set; }
    }
}
