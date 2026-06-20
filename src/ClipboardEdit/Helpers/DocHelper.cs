using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace ClipboardEdit.Helpers
{
    public static class DocHelper
    {
        private static async Task<string> ParseMarkdown(string md)
        {
            List<JsonDocInfo> infos = new List<JsonDocInfo>();

            // Parse the markdown into an AST using Markdig.
            var doc = Markdown.Parse(md,
                new MarkdownPipelineBuilder()
                .UsePipeTables()
                .UseGridTables()
                .Build());

            // Find the "Constants" header.
            var hdr = doc.FirstOrDefault(x => x is HeadingBlock h && (h.Inline?.FirstChild.ToString().Equals("Constants",
                StringComparison.OrdinalIgnoreCase) ?? false)) as HeadingBlock;

            if (hdr != null)
            {
                var after = doc.SkipWhile(x => x != hdr).Skip(1);
                var table = after.OfType<Table>().FirstOrDefault();

                if (table != null)
                {
                    // Skip the header.
                    var rows = table.OfType<TableRow>().Skip(1);

                    foreach (var row in rows)
                    {
                        var nameCell = (TableCell)row.First();
                        var descCell = (TableCell)row.Last();
                        var info     = new JsonDocInfo();

                        foreach (var block in nameCell)
                            if (block is ParagraphBlock para)
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (var emphasis in para.Inline.OfType<EmphasisInline>())
                                    foreach (var child in emphasis)
                                        if (child is LiteralInline literal)
                                            sb.Append(literal.Content.ToString());

                                info.Name = sb.ToString();
                            }

                        foreach (var block in descCell)
                            if (block is ParagraphBlock para)
                            {
                                StringBuilder sb = new StringBuilder();
                                bool structFound = false;

                                // WHY DO THE LINKS NOT WORK
                                foreach (var inline in para.Inline)
                                    if (inline is LiteralInline literal)
                                    {
                                        sb.Append(literal.Content.ToString());
                                    }
                                    else if (inline is EmphasisInline emphasis)
                                    {
                                        foreach (var child in emphasis)
                                            if (child is LiteralInline l)
                                                sb.Append(l.Content.ToString());
                                    }
                                    else if (inline is LinkInline link)
                                    {
                                        string content = string.Empty;

                                        foreach (var c in link)
                                            if (c is LiteralInline li)
                                                content += li.Content.ToString();
                                            else if (c is EmphasisInline em)
                                                foreach (var cc in em)
                                                    if (cc is LiteralInline lit)
                                                        content += lit.Content.ToString();

                                        sb.Append(content);
                                        if (content.All(x => char.IsUpper(x)))
                                        {
                                            if (structFound) continue;
                                            structFound = true;

                                            // This must be a struct, so find its docs.
                                            string url = $"https://raw.githubusercontent.com/MicrosoftDocs/sdk-api/refs" +
                                                $"/heads/docs/sdk-api-src/content/wingdi/ns-wingdi-{content.ToLowerInvariant()}.md";

                                            using (HttpClient client = new HttpClient())
                                            {
                                                var result = await client.GetAsync(url);

                                                if (result.IsSuccessStatusCode)
                                                {
                                                    var markdown = await result.Content.ReadAsStringAsync();
                                                    var index    = markdown.IndexOf('#'); // find the first header

                                                    if (index != -1)
                                                        info.AdditionalStructDocs = markdown.Substring(index);
                                                    else
                                                        info.AdditionalStructDocs = markdown;
                                                }
                                            }
                                        }
                                    }

                                info.Description = sb.ToString();
                            }

                        infos.Add(info);
                    }
                }
            }

            return JsonConvert.SerializeObject(infos, Formatting.Indented);
        }

        public static async Task<HttpRequestResultInfo> UpdateDocsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("https://raw.githubusercontent.com/MicrosoftDocs/win32/refs" +
                    "/heads/docs/desktop-src/dataxchg/standard-clipboard-formats.md");

                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClipboardEdit");
                        
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        File.WriteAllText(Path.Combine(dir, "FormatDocData.json"),
                            await ParseMarkdown(await result.Content.ReadAsStringAsync()));
                    }
                    catch { }
                }
                
                return new HttpRequestResultInfo(result);
            }
        }

#nullable enable

        private static string? LoadDocText()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ClipboardEdit", "FormatDocData.json");

            if (File.Exists(path))
                return File.ReadAllText(path);

            return null;
        }

        public static List<JsonDocInfo> LoadDocs()
        {
            string? doc = LoadDocText();

            if (doc != null)
                return JsonConvert.DeserializeObject<List<JsonDocInfo>>(doc) ?? [];


            return [];
        }
    }

    public class JsonDocInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdditionalStructDocs { get; set; } = string.Empty;
    }

    public struct HttpRequestResultInfo
    {
        public HttpStatusCode Code { get; set; }
        public bool Success { get; set; }
        public string Reason { get; set; } = string.Empty;
        
        public HttpRequestResultInfo(HttpResponseMessage resp)
        {
            Code = resp.StatusCode;
            Success = resp.IsSuccessStatusCode;
            Reason = resp.ReasonPhrase;
        }
    }
}
