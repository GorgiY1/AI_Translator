using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Services
{
    public interface ITextToSpeechService
    {
        Task<Stream> TextToSpeechAsync(string text, string language);
    }
}
