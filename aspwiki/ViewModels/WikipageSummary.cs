﻿using ASPWiki.Model.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASPWiki.ViewModels
{
    public class WikipageSummary
    {
        public string Title { get; set; }

        public string Path { get; set; }

        [UIHint("Label")]
        public Label Label { get; set; }

        public string ContentSummary { get; set; }

        public string Author { get; set; }

        public string Size { get; set; }

        public bool Public { get; set; }

        public int AttachmentCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastModified { get; set; }
    }
}
