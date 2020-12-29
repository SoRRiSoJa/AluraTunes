using AluraTunes.Data;
using AluraTunes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AluraTunes
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Genero> generos = ListGeneros();
            List<Musica> musicas = ListMusicas();

            var generoFiltrado = (from g in generos where g.Nome.Contains("Rock") select g.Nome).ToList();

            var musicasFiltradas = (from musica in musicas join genero in generos on musica.GeneroId equals genero.GeneroId
                                    select new { musica.Nome, Genero = genero.Nome }).ToList();
            XElement root = LoadXMLFile();
            
            var queryuXml = (from ge in root.Element("Generos").Elements("Genero") select new { Id=ge.Element("GeneroId").Value,Genero=ge.Element("Nome").Value }).ToList();
            var query = (from g in root.Element("Generos").Elements("Genero") join m in root.Element("Musicas").Elements("Musica")
                           on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                         select new { Nome = m.Element("Nome").Value, Genero = g.Element("Nome").Value }).ToList();
            using (var context = new Context()) 
            {
                var listaGeneros = (from g in context.Generos select new { g.GeneroId,g.Nome }).ToList();
            }
        }

        private static XElement LoadXMLFile()
        {
            return XElement.Load(@"C:\Users\joao.silva\Documents\Alura\AluraTunes\AluraTunes\Files\Musicas.xml");
        }

        private static List<Musica> ListMusicas()
        {
            return new List<Musica>()
            {
                new Musica() { Id = 1, Nome = "Sweet Child O' Mine ",GeneroId=1},
                new Musica() { Id = 2, Nome = "I Shoot The Sheriff",GeneroId=2},
                new Musica() { Id = 3, Nome = "Danúbio Azul",GeneroId=5}
            };
        }

        private static List<Genero> ListGeneros()
        {
            return new List<Genero>()
            {
                new Genero() { GeneroId = 1, Nome = "Rock" },
                new Genero() { GeneroId = 2, Nome = "Regae" },
                new Genero() { GeneroId = 3, Nome = "Rock Progresivo" },
                new Genero() { GeneroId = 4, Nome = "Punk Rock" },
                new Genero() { GeneroId = 5, Nome = "Classica" }
            };
        }
    }
}
