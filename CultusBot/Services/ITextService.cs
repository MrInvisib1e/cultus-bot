using CultusBot.Data.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultusBot.Services
{
    public interface ITextService
    {
        string GetRandomText(TextType textType);
    }
}
