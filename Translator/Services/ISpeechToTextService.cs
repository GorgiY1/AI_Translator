using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Services
{
    public interface ISpeechToTextService
    {
        Task<string> SpeechToTextAsync(Stream audioStream);
    }

}
