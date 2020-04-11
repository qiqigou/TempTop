using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public interface ITempBuild
    {
        void LoadFromFile(string path);
        void LoadFromJson(string json);
        void LoadFromObject(object obj);
        string Execute();
        string Execute(object data);
        string Execute(string json);
    }
}
