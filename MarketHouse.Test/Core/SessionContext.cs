using NHibernate;
using NHibernate.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace MarketHouse.Test.Core
{
    public class SessionTest : IDbSessionManager
    {
        private Dictionary<string, IEnumerable<object>> listaTabelleDb = new Dictionary<string, IEnumerable<object>>();

        private string GetKey<T>()
        {
            return typeof(T).FullName; ;
        }

        private string GetKey(object obj)
        {
            return obj.GetType().FullName;
        }

        private bool ContainsKey(string key)
        {
            return this.listaTabelleDb.ContainsKey(key);
        }

        private List<object> GetListaTabelle(string key)
        {
            if (ContainsKey(key))
                return listaTabelleDb.Where(x => x.Key == key).Select(x => x.Value).First().ToList();
            else
                return new List<object>();
        }

        private void AddRecords(List<object> lista, string key)
        {
            if (ContainsKey(key))
                this.listaTabelleDb.Remove(key);

            this.listaTabelleDb.Add(key, lista);
        }

        public void AddRecords<T>(List<T> lista)
        {
            string key = GetKey<T>();
            AddRecords(lista.Cast<object>().ToList(), key);
        }

        public void AddRecord<T>(T obj)
        {
            string key = GetKey<T>();

            if (ContainsKey(key))
            {
                List<T> listaAggiunta = this.listaTabelleDb.Where(x => x.Key == key).Select(x => x.Value).First().Cast<T>().ToList();
                listaAggiunta.Add(obj);

                AddRecords<T>(listaAggiunta);
            }
            else
            {
                List<T> nuovaLista = new List<T>();
                nuovaLista.Add(obj);
                AddRecords<T>(nuovaLista);
            }
        }

        public override IQueryable<T> Query<T>()
        {
            string key = GetKey<T>();
            if (ContainsKey(key))
                return listaTabelleDb.Where(x => x.Key == key).Select(x => x.Value).First().Cast<T>().AsQueryable();
            else
                return new List<T>().AsQueryable();
        }

        public override void Persist(object obj)
        {
            SaveOrUpdate(obj);
        }

        public override object Save(object obj)
        {
            SaveOrUpdate(obj);
            return new Random().Next();
        }

        public override void SaveOrUpdate(object obj)
        {
            string key = GetKey(obj);

            List<object> listaPerChiave = GetListaTabelle(key);

            if (listaPerChiave.Contains(obj))
                listaPerChiave[listaPerChiave.IndexOf(obj)] = obj;
            else
                listaPerChiave.Add(obj);

            AddRecords(listaPerChiave, key);
        }

        public override void Update(object obj)
        {
            string key = GetKey(obj);

            List<object> listaPerChiave = GetListaTabelle(key);
            listaPerChiave[listaPerChiave.IndexOf(obj)] = obj;

            AddRecords(listaPerChiave, key);

        }

        public override void Delete(object obj)
        {
            string key = GetKey(obj);

            List<object> listaPerChiave = GetListaTabelle(key);
            listaPerChiave.Remove(obj);

            AddRecords(listaPerChiave, key);
        }
    }
}
