﻿using System;
using System.Collections.Generic;
using ASPWiki.Model;

namespace ASPWiki.Services
{
    public class WikiRepository : IWikiRepository
    {
        private Dictionary<string, WikiPage> wikiRepository;

        public WikiRepository()
        {
            wikiRepository = new Dictionary<string, WikiPage>();
        }

        public void Delete(string title)
        {
            wikiRepository.Remove(title);
        }

        public bool Exists(string title)
        {
            return wikiRepository.ContainsKey(title);
        }

        public WikiPage Get(string title)
        {
            return wikiRepository[title];
        }

        public void Save(string title, WikiPage wikiPage)
        {
            wikiRepository.Add(title, wikiPage);
        }
    }
}
