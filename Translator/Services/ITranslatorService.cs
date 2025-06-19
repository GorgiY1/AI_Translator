using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Services
{
    public interface ITranslatorService
    {
        Task<string> TranslateTextAsync(string text, string toLanguage, string fromLanguage = null);
    }
}

