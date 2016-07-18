﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ASPWiki.Model
{
    public class WikiPage
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime LastModified { get; set; }

        public List<string> Path { get; set; }

        public string Parent
        {
            get
            {
                string parent = string.Empty;
                if (Path.Count >= 2)
                {
                    parent = Path[Path.Count - 2];
                }

                return parent;
            }
        }

        public WikiPage()
        {
            if (Id == null)
            {
                Id = Guid.NewGuid();
            }
        }

        public WikiPage(string title)
        {
            Id = Guid.NewGuid();

            this.Title = title;
            Path = new List<string>(new string[]{ Title });
        }

        public void SetPath(List<string> ParentPath)
        {
            if (ParentPath != null) {
                Path = new List<string>(ParentPath);
                Path.Add(Title);
            }
            else
            {
                Path = new List<string>(new string[] { Title });
            }
        }

        public string GetPathString()
        {
            string path = String.Empty;

            for (int i = 0; i < Path.Count; i++)
            {
                path += Path[i];

                if (i != Path.Count - 1)
                    path += "/";
            }

            return path;
        }

        public string GetPathToParent()
        {
            string path = String.Empty;

            for (int i = 0; i < Path.Count - 1; i++)
            {
                path += Path[i];

                if (i != Path.Count - 2)
                    path += "/";
            }

            return path;
        }

        private const int SUMMARY_LENGTH = 200;
        public string GetContentSummary()
        {
            if (Content == null || Content.Length <= 0)
                return string.Empty;

            int length = Content.Length < SUMMARY_LENGTH ? Content.Length : SUMMARY_LENGTH;

            Regex rgx = new Regex(@"<\/[a-zA-Z""]*>");
            var match = rgx.Match(Content.Substring(length));
            string s = Content.Substring(0, length + match.Index + match.Length);

            s = s.Replace("<h1>", "<h4>").Replace("</h1>", "</h4>");
            s = s.Replace("<h2>", "<h4>").Replace("</h2>", "</h4>");
            s = s.Replace("<h3>", "<h4>").Replace("</h3>", "</h4>");

            return s;
        }

        public string GetSizeKiloBytes()
        {
            if (Content == null)
                return "0 KB";

            return (Content.Length * sizeof(char) / 1024f).ToString("0.00", new CultureInfo("fi")) + " KB";
        }

        public override string ToString()
        {
            return "Title: " + Title + " Content: " + Content + " Path: " + GetPathString();
        }
    }
}