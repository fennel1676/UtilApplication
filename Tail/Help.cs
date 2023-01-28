using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tail
{
    public class Help
    {
        public string AnalyzeArgument(string[] args)
        {
            if (null == args || 1 <= args.Count())
                return "프로그램을 동작시킬 파라메터가 부족합니다.";

            for(int i = 0; i < args.Count(); i++)
            {
                switch (args[i])
                {
                    case "-t": break;
                    default: return args[i] + "은(는) 내부 또는 외부 명령이 아닙니다.";
                }
            }

            return string.Empty;
        }
    }
}
