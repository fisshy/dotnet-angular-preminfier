using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
namespace ng_bundle
{
    public class AngularPreMinifierTransform : IBundleTransform
    {

        public void Process(BundleContext context, BundleResponse response)
        {
            var preMinifier = new AngularPreMinifier(response.Content);
            response.Content = preMinifier.Parse();
        }
    }

    public class AngularPreMinifier
    {
        public AngularPreMinifier(string source)
        {
            this.Source = source;
        }
        private string Source { get; set; }
        private string NewSource { get; set; }
        public List<string> Params = new List<string>();
        public List<string> Functions = new List<string>();

        public string Parse()
        {
            var current = string.Empty;
            var insideFunction = false;
            var param = string.Empty;
            var doingParams = false;
            var completeParams = false;
            var openingBrackets = 0;

            foreach (var c in Source)
            {
                NewSource += c;
                current += c;

                if (!insideFunction && current.Contains("function"))
                {
                    insideFunction = true;
                    current = "function";
                }

                if (insideFunction)
                {
                    if (!doingParams && !completeParams)
                        doingParams = c == '(';

                    if (doingParams)
                    {
                        HandleParams(ref current, ref param, ref doingParams, ref completeParams, c);
                    }
                    else
                    {
                        var functionCreated = HandleFunction(ref current, ref openingBrackets, c);
                        if (functionCreated)
                        {

                            //Clean up
                            completeParams = false;
                            insideFunction = false;
                            doingParams = false;
                            openingBrackets = 0;
                        }
                    }
                }
            }

            return NewSource;
        }

        private bool HandleFunction(ref string current, ref int openingBrackets, char c)
        {

            if (c == '{')
            {
                openingBrackets += 1;
            }

            if (c == '}' && openingBrackets == 1)
            {
                Functions.Add(current);
                current = string.Empty; ;
                AngularInject();

                return true;
            }

            if (c == '}' && openingBrackets > 1)
            {
                openingBrackets -= 1;
            }

            return false;
        }

        private void HandleParams(ref string current, ref string param, ref bool doingParams, ref bool completeParams, char c)
        {
            if (c != '(' && c != ')' && c != ',')
            {
                param += c;
            }

            if (c == ',')
            {
                Params.Add("\"" + param.Trim() + "\"");
                param = string.Empty;
            }
            else if (c == ')')
            {

                Params.Add("\"" + param.Trim() + "\"");
                param = string.Empty;
                doingParams = false;
                completeParams = true;
            }
        }

        public void AngularInject()
        {
            if (Params.Count == 0)
            {
                Functions.Clear();
                return;
            }

            var function = Functions[0];
            var param = Params;

            var toReplace = string.Concat('[', string.Join(",", param), ',', function, ']');

            Functions.Clear();
            Params.Clear();

            NewSource = NewSource.Replace(function, toReplace);

        }
    }


}

