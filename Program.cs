
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;



namespace Quiz3
{
    class Program
    {
        static Regex header = new Regex(@"<h2>", RegexOptions.IgnoreCase);
        static Regex urlMatch = new Regex(@"href=\" + '"' + @"*(?<address>[\w.:\?=\+\~/\-_]+.html)\" + '"' + @"*[>\s]", RegexOptions.IgnoreCase);
        static Regex cssMatch = new Regex(@"href=\" + '"' + @"*(?<address>[\w.:\?=\+\~/\-_]+.css)\" + '"', RegexOptions.IgnoreCase);
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ProgramName <URL>");
                return;
            }
            int count = 0;
            List<String> finalList = new List<String>();
            getAddressesFromSite(args[0], ref count, ref finalList);

            StreamWriter stream = new StreamWriter("listOfPages.txt");
            foreach (String element in finalList)
            {
                stream.WriteLine(element);
            }
            stream.Close();
        }

        static void getAddressesFromSite(String url, ref int count, ref List<String> linksParsed)
        {
            int thisCount = count + 0;
            HttpWebRequest currentSite = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse currentResponse;
            try { 
            currentResponse = (HttpWebResponse)currentSite.GetResponse();
            }
            catch (Exception e)
            {
                return;
            }
            
            Stream currentBinStream = currentResponse.GetResponseStream();
            StreamReader currentTextStream = new StreamReader(currentBinStream);

            String currentHTML = currentTextStream.ReadToEnd();
            Match matchHeader = header.Match(currentHTML);
            currentHTML = currentHTML.Insert(matchHeader.Index + matchHeader.Length, thisCount + ") ");
            Match matchCSS = cssMatch.Match(currentHTML);
            if (matchCSS.Success) { 
                currentHTML = currentHTML.Replace(matchCSS.Value, "href=\"style.css\"");
            }
            //if (count < 10)
            //{


                MatchCollection currentLinks = urlMatch.Matches(currentHTML);
                
                foreach (Match currentMatch in currentLinks)
                {
                    String currentURL = currentMatch.Groups["address"].Value;
                    
                    if (currentURL.StartsWith("http://"))
                    {
                        continue;
                    }
                    String fullURL = url.Substring(0, url.LastIndexOf("/") + 1);
                    String tempURL = currentURL;
                    if (tempURL.StartsWith("http:"))
                    {
                        tempURL = tempURL.Substring(5);
                    }
                    while (tempURL.StartsWith("../"))
                    {
                        fullURL = fullURL.Substring(0, fullURL.Length - 1);
                        fullURL = fullURL.Substring(0, fullURL.LastIndexOf("/") + 1);
                        tempURL = tempURL.Substring(3);
                    }
                    fullURL += tempURL;
                    if (!linksParsed.Contains(fullURL))
                    {
                        linksParsed.Add(fullURL);
                    }
                    else
                    {
                        int existingCount = linksParsed.IndexOf(fullURL) + 1; //zero is the homepage
                        if (currentMatch.Value.EndsWith(" "))
                        {

                            currentHTML = currentHTML.Replace(currentMatch.Value, "href=" + String.Format("\"{0:0000}.html\"", existingCount));
                        }
                        else
                        {
                            currentHTML = currentHTML.Replace(currentMatch.Value, "href=" + String.Format("\"{0:0000}.html\">", existingCount) + "<b>(Page " + existingCount + ")</b> ");
                        }
                        continue;
                    }
                    
                    count++;
                    Console.WriteLine(fullURL);
                    if (currentMatch.Value.EndsWith(" "))
                    {

                        currentHTML = currentHTML.Replace(currentMatch.Value, "href=" + String.Format("\"{0:0000}.html\"", count));
                    }
                    else { 
                        currentHTML = currentHTML.Replace(currentMatch.Value, "href=" + String.Format("\"{0:0000}.html\">", count) + "<b>(Page " + count + ")</b> ");
                    }
                    getAddressesFromSite(fullURL, ref count, ref linksParsed);
                }

                String fname = String.Format("{0:0000}.html", thisCount);
                File.WriteAllText(fname, currentHTML);
            //}

            currentTextStream.Close();
            currentBinStream.Close();
            currentResponse.Close();


            
        }
    }
}
