using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempTop
{
    public interface ITempBuild
    {
        void LoadFromFile(string path);
        void LoadFromJson(string json);
        void LoadFromObject(object obj);
        string Execute();

    }
}
