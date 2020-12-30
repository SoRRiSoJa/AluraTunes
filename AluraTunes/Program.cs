using AluraTunes.Data;
using AluraTunes.Extensions;
using AluraTunes.Model;
using Microsoft.EntityFrameworkCore;
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
            FiltrarGeneros(generos);
            FiltrarMusicas(generos, musicas);
            //--
            GenerosDb();
            HandleXML();
            //---
            var albuns = GetAlbunsFiltrados("Led", "You Shook Me");
            var total = TotalVendasArtista("Led Zeppelin");
            //var relatorio = RelatorioMaisVendidosPorArtista("Led Zeppelin");
            ///--

            var resumo = RelatorioVendasResumido();
            using (var db = new Context())
            {
                var mediana=db.NotaFiscal.Median((nf)=>nf.Total);
            }
        }

        

        public static dynamic RelatorioVendasResumido()
        {
            using (var db = new Context())
            {
                return (from nf in db.NotaFiscal group nf by 1 into grp select new { MaiorVenda = grp.Max((mv) => mv.Total), MenorVenda = grp.Min((mv) => mv.Total), MediaVenda = grp.Average((mv) => mv.Total) }).SingleOrDefault();
            }
        }
        private static void HandleXML()
        {
            XElement root = LoadXMLFile();

            var queryuXml = (from ge in root.Element("Generos").Elements("Genero") select new { Id = ge.Element("GeneroId").Value, Genero = ge.Element("Nome").Value }).ToList();
            var query = (from g in root.Element("Generos").Elements("Genero")
                         join m in root.Element("Musicas").Elements("Musica")
                            on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                         select new { Nome = m.Element("Nome").Value, Genero = g.Element("Nome").Value }).ToList();
        }

        private static void GenerosDb()
        {
            using (var context = new Context())
            {
                var listaGeneros = (from g in context.Genero select new { g.GeneroId, g.Nome }).ToList();
            }
        }

        private static void FiltrarMusicas(List<Genero> generos, List<Musica> musicas)
        {
            var musicasFiltradas = (from musica in musicas
                                    join genero in generos on musica.GeneroId equals genero.GeneroId
                                    select new { musica.Nome, Genero = genero.Nome }).ToList();
        }

        private static void FiltrarGeneros(List<Genero> generos)
        {
            var generoFiltrado = (from g in generos where g.Nome.Contains("Rock") select g.Nome).ToList();
        }

        public static List<dynamic> RelatorioMaisVendidosPorArtista(string nomeArtista)
        {
            using (var db = new Context(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\joao.silva\Documents\Alura\AluraTunes\AluraTunes\Data\AluraTunes.mdf;Integrated Security=True"))
            {
                return (from inf in db.ItemNotaFiscal
                        where inf.Faixa.Album.Artista.Nome == nomeArtista
                        group inf by inf.Faixa.Album into grp
                        let vendasPorAlbum = grp.Sum((g) => g.PrecoUnitario * g.Quantidade)
                        orderby vendasPorAlbum descending
                        select new
                        {
                            TituloAlbum = grp.Key.Titulo,
                            NomeArtista = grp.Key.Artista.Nome,
                            Total = vendasPorAlbum
                        }
                      ).ToList<dynamic>();
            }
        }
        /// <summary>
        /// Recupera o total de vendas para um determinado artista
        /// </summary>
        /// <param name="nomeartista"></param>
        /// <returns></returns>
        public static decimal TotalVendasArtista(string nomeartista)
        {
            using (var db = new Context(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\joao.silva\Documents\Alura\AluraTunes\AluraTunes\Data\AluraTunes.mdf;Integrated Security=True"))
            {
                return db.ItemNotaFiscal.Where((inf) => inf.Faixa.Album.Artista.Nome == nomeartista).Sum((inf) => inf.PrecoUnitario);
            }
        }
        /// <summary>
        /// Recupera todos os Albuns que contenham uma determinada musica 
        /// </summary>
        /// <param name="album"></param>
        /// <param name="nomeMusica"></param>
        /// <returns></returns>
        private static List<dynamic> GetAlbunsFiltrados(string album, string nomeMusica)
        {
            if (!string.IsNullOrEmpty(album) && !string.IsNullOrEmpty(nomeMusica))
            {
                using (var db = new Context())
                {
                    return (from alb in db.Album
                            join fa in db.Faixa on alb.AlbumId equals fa.AlbumId
                            where alb.Artista.Nome.Contains(album) && fa.Nome == nomeMusica
                            select new
                            {
                                alb.Titulo,
                                alb.Artista.Nome,
                                alb.Faixas,
                                TotalFaixas = alb.Faixas.Count()
                            }
                            ).ToList<dynamic>();
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Recupera todas as faixas de um determinado artista
        /// </summary>
        /// <param name="nomeArtista"></param>
        /// <returns></returns>
        private static List<dynamic> GetFaixasArtista(string nomeArtista)
        {
            using (var db = new Context())
            {
                return (from fa in db.Faixa
                        where fa.Album.Artista.Nome.Contains(nomeArtista)
                        select
new { NomeAlbum = fa.Album.Artista.Nome, Artista = fa.Nome }).OrderBy((f) => f.NomeAlbum).ThenBy((f) => f.Artista).ToList<dynamic>();
            }
        }
        /// <summary>
        /// Carrega um arquivo XML
        /// </summary>
        /// <returns></returns>
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
