using System;
using System.Collections;

using GameGuy.GGCrypt;

namespace GameGUY {
    public class GGInterpreter {
        private string programName;
        private readonly ArrayList programData = new ArrayList();
        private int screenNum = 0;

        private bool gameOver = false;

        public bool getGameState() {
            return this.gameOver;
        }

        public void reset() {
            screenNum = 0;
            gameOver = false;
        }

        public GGInterpreter(string fileName, string desKey) {
            this.parseProgram(fileName, desKey);
        }

        public string getProgramName() {
            return this.programName;
        }

        public string render() {
            if (!gameOver && programData.Count > screenNum) {
                return programData[screenNum].ToString();
            } else {
                gameOver = true;
                return "Game Over";
            }
        }

        public void sendInput(string keyEntered) {
            if (programData.Count > screenNum) {
                this.screenNum++;
            } else if (programData.Count == screenNum) {
                this.gameOver = true;
            }
        }

        private void parseProgram(string fileName, string key) {
            string tmpStr = "", tmpProgramStr = "", tmpCmd = "", tmpData = "";

            String decStr = Decrypt.decryptFile(fileName, key);
            decStr = decStr.Replace("\n", "");
            String[] strArr = decStr.Split('\r');

            this.programName = strArr[0];

            for (int x = 1; x < strArr.Length; x++) {
                tmpStr = strArr[x];

                if (tmpStr.IndexOf(' ') != -1) {
                    tmpCmd = tmpStr.Substring(0, tmpStr.IndexOf(' '));

                    switch (tmpCmd.ToUpper()) {
                        case "PRINT":
                            tmpData = tmpStr.Substring(tmpStr.IndexOf(' '));
                            tmpData = tmpData.Remove(0, 1);
                            tmpData = tmpData.Remove(tmpData.Length - 1);

                            tmpProgramStr += tmpData.Replace("\"", "") + "\r\n";
                            break;
                        case "INPUT":
                            this.programData.Add(tmpProgramStr);

                            tmpProgramStr = "";
                            tmpData = "";

                            break;
                    }
                }
            }
        }
    }
}