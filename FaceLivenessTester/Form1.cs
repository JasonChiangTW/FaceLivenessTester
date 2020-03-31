using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace FaceLivenessTester {
    public partial class fmMain : Form {
        public fmMain() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;            
        }
        private class ResultData {
            public string livness;
            public string megliveFMP;
        }

        private Dictionary<string, ResultData> dicResult = new Dictionary<string, ResultData>();

        async void DoHttpPost() {            
            string[] filesPath = System.IO.Directory.GetFiles(openFD.SelectedPath);
            foreach (string s in filesPath) {
                if (s.Contains("jpg")) {
                    postManForLiveness(s);
                    postManForMegliveFMP(s);
                    await Task.Delay(50);
                }
            }
            btnExport.Visible = true;
            MessageBox.Show("執行完畢，請點擊『匯出』以取得結果");
        }

        private void btnExcute_Click(object sender, EventArgs e) {
            try {
                lbResult.Items.Clear();
                DoHttpPost();                           
            } catch (Exception) {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void postManForMegliveFMP (string filePath) {
            try {
                string url = "http://210.63.218.35:9007/meglive";
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("rotate", "1");
                string result = HttpPostData(url, 2000, "img", filePath, nvc);
        
                if (dicResult.ContainsKey(filePath)) {
                    dicResult[filePath].megliveFMP = result;
                } else {
                    dicResult.Add(filePath, new ResultData() { megliveFMP = result, livness = string.Empty });
                }
                lbResult.Items.Insert(0, filePath + "=>" + result);
            } catch (Exception) {
                //MessageBox.Show(ex.ToString());
            }
        }
        private void postManForLiveness(string filePath) {
            try {
                string url = "http://210.63.218.35:9004/faceid/v1/liveness_cfg";
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("rotate", "1");
                string result = HttpPostData(url, 2000, "img", filePath, nvc);
                
                if (dicResult.ContainsKey(filePath)) {
                    dicResult[filePath].livness = result;
                } else {
                    dicResult.Add(filePath, new ResultData() { livness = result, megliveFMP = string.Empty });
                }
                ThreadStart ts = () => updateResultInTool(filePath,result);
                Thread thread = new Thread(ts);
                thread.Start();
            } catch (Exception) {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void updateResultInTool(string filePath, string result) {
            lbResult.Items.Insert(0, filePath + "=>" + result);
        }

        private static string HttpPostData(string url, int timeOut, string fileKeyName,
                                    string filePath, NameValueCollection stringDict) {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem)) {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8"))) {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }


        private void generateHtmlResult() {
            try {
                string resultFileName = "Result_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
                StringBuilder sb = new StringBuilder();
                sb.Append("<!DOCTYPE html>");
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<style>");
                sb.Append("#customers {");
                sb.Append("  font-family: \"Trebuchet MS\", Arial, Helvetica, sans-serif;");
                sb.Append("  border-collapse: collapse;");
                sb.Append("  width: 100%;text-align:center;");
                sb.Append("}");
                sb.Append("");
                sb.Append("#customers td, #customers th {");
                sb.Append("  border: 1px solid #ddd;");
                sb.Append("  padding: 8px;");
                sb.Append("}");
                sb.Append("");
                sb.Append("#customers tr:nth-child(even){background-color: #f2f2f2;}");
                sb.Append("");
                sb.Append("#customers tr:hover {background-color: #ddd;}");
                sb.Append("");
                sb.Append("#customers th {");
                sb.Append("  padding-top: 12px;");
                sb.Append("  padding-bottom: 12px;");
                sb.Append("  text-align: left;");
                sb.Append("  background-color: #4CAF50;");
                sb.Append("  color: white;");
                sb.Append("}");
                sb.Append("</style>");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<html>");
                sb.Append("<table  id=\"customers\">");
                sb.Append("<tr><th>項次</th><th>照片</th><th>MegliveFMP</th><th>liveness</th><th>一致</th></tr>");
                int idx = 0;
                foreach (KeyValuePair<string, ResultData> kvp in dicResult) {
                    idx++;
                    bool passLiveness = false;
                    bool passFMP = false;
                    sb.Append("<tr>");
                    sb.Append("<td>idx_").Append(idx).Append("</td>");
                    sb.Append("<td>");
                    sb.Append("<img style='height:300px;display:none;' src=\"" + kvp.Key + "\">");
                    sb.Append("</td>");
                    string color = "red";
                    if (kvp.Value.megliveFMP.Contains("PASS")) {
                        passFMP = true;
                        color = "green";
                    }                    
                    sb.Append("<td><label style='color:").Append(color).Append("'>").Append(kvp.Value.megliveFMP).Append("</label></td>");
                    color = "red";
                    if (kvp.Value.livness.Contains("\"error\":\"\"")) {
                        passLiveness = true;
                        color = "green";
                    }                    
                    sb.Append("<td><label style='color:").Append(color).Append("'>").Append(kvp.Value.livness).Append("</label></td>");
                    sb.Append("<td>").Append(passLiveness.Equals(passFMP) ? string.Empty : "<label style='color:green'>#</label>").Append("</tr>");
                    sb.Append("</tr>");
                }                
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(openFD.SelectedPath + "\\"+ resultFileName)) {
                    file.WriteLine(sb.ToString());
                }
                MessageBox.Show("匯出成功，結果檔案位置：" + openFD.SelectedPath + "\\" + resultFileName);
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }


        private void btnGetFolder_Click(object sender, EventArgs e) {
            if (openFD.ShowDialog() == DialogResult.OK) {
                txtFolder.Text = openFD.SelectedPath;
            }
            btnExcute.Visible = true;
        }

        private void btnExport_Click(object sender, EventArgs e) {
            generateHtmlResult();

        }
    }
}
