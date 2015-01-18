using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jp.ktsystem.kadai201411.h_okita;

namespace jp.ktsystem.kadai201411.h_okita_Test
{
    [TestClass]
    public class Lv1Test
    {
        //テストデータフォルダ
        private static readonly string TEST_FOLDER_PATH = "..\\..\\..\\TestData\\Lv.1\\";
        //入力フォルダ名
        private static readonly string INPUT_FOLDER_NAME = "input";
        //出力フォルダ名
        private static readonly string OUTPUT_FOLDER_NAME = "output";
        //比較対象フォルダ名
        private static readonly string EXPECTED_FOLDER_NAME = "expected";
        //出力ファイル名
        private static readonly string OUTPUT_FILE_NAME = "ordercount.out";


        [TestMethod]
        public void Lv1N001()
        {
            string path = TEST_FOLDER_PATH + "001\\";
            AssertEquals(path, 0);
        }


        [TestMethod]
        public void Lv1N002()
        {
            string path = TEST_FOLDER_PATH + "002\\";
            AssertEquals(path, 0);
        }

        [TestMethod]
        public void Lv1N003()
        {
            string path = TEST_FOLDER_PATH + "003\\";
            AssertEquals(path, 4);
        }

        [TestMethod]
        public void Lv1E101()
        {
            string path = TEST_FOLDER_PATH + "101\\";
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(null, outputDir, -1);
        }

        [TestMethod]
        public void Lv1E102()
        {
            string path = TEST_FOLDER_PATH + "102\\";
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(string.Empty, outputDir, -1);
        }

        [TestMethod]
        public void Lv1E103()
        {
            string path = TEST_FOLDER_PATH + "103\\";
            //受注フォルダ
            string orderDir = path + "dummy";
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -1);
        }

        [TestMethod]
        public void Lv1E104()
        {
            string path = TEST_FOLDER_PATH + "104\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E105()
        {
            string path = TEST_FOLDER_PATH + "105\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E106()
        {
            string path = TEST_FOLDER_PATH + "106\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E107()
        {
            string path = TEST_FOLDER_PATH + "107\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E108()
        {
            string path = TEST_FOLDER_PATH + "108\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E109()
        {
            string path = TEST_FOLDER_PATH + "109\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E110()
        {
            string path = TEST_FOLDER_PATH + "110\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + OUTPUT_FOLDER_NAME;

            AssertFail(orderDir, outputDir, -2);
        }

        [TestMethod]
        public void Lv1E111()
        {
            string path = TEST_FOLDER_PATH + "111\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;

            AssertFail(orderDir, null, -8);
        }

        [TestMethod]
        public void Lv1E112()
        {
            string path = TEST_FOLDER_PATH + "112\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;

            AssertFail(orderDir, string.Empty, -8);
        }

        [TestMethod]
        public void Lv1E113()
        {
            string path = TEST_FOLDER_PATH + "113\\";
            //受注フォルダ
            string orderDir = path + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = path + "dummy";

            AssertFail(orderDir, outputDir, -8);
        }

        /// <summary>
        /// 正常時実行
        /// </summary>
        /// <param name="testDir">テストデータフォルダディレクトリ</param>
        /// <param name="returnNo">戻り値</param>
        private void AssertEquals(string testDir, int returnNo)
        {
            //受注フォルダ
            string orderDir = testDir + INPUT_FOLDER_NAME;
            //出力フォルダ
            string outputDir = testDir + OUTPUT_FOLDER_NAME;

            //対象メソッド実行
            int result = Kadai.CountOrder(orderDir, outputDir);
            if (returnNo != result)
            {
                Assert.Fail(string.Format("戻り値は{0}であることが期待されましたが{1}が返されました。", returnNo, result));
            }

            string anOutputFilePath = testDir + OUTPUT_FOLDER_NAME + "/" + OUTPUT_FILE_NAME;
            if (!File.Exists(anOutputFilePath))
            {
                Assert.Fail("出力ファイルが存在しませんでした。");
            }

            string anExpextedFilePath = testDir + EXPECTED_FOLDER_NAME + "/" + OUTPUT_FILE_NAME;
            FileStream fs1 = new FileStream(anOutputFilePath, FileMode.Open);
            FileStream fs2 = new FileStream(anExpextedFilePath, FileMode.Open);
            int byte1;
            int byte2;

            try
            {
                if (fs1.Length == fs2.Length)
                {
                    byte1 = fs1.ReadByte();
                    byte2 = fs2.ReadByte();
                    if (byte1 != byte2)
                    {
                        Assert.Fail("出力ファイルの中身が異なります。");
                    }

                    while (byte1 != -1 || byte2 != -1)
                    {
                        byte1 = fs1.ReadByte();
                        byte2 = fs2.ReadByte();
                        if (byte1 != byte2)
                        {
                            Assert.Fail("出力ファイルの中身が異なります。");
                        }
                    }
                }
                else
                {
                    Assert.Fail("出力ファイルの中身が異なります。");
                }
            }
            finally
            {
                fs1.Close();
                fs2.Close();
            }
        }

        /// <summary>
        /// 異常時実行
        /// </summary>
        /// <param name="orderDir">入力フォルダパス</param>
        /// <param name="outputDir">出力フォルダパス</param>
        /// <param name="returnNo">戻り値</param>
        private void AssertFail(string orderDir, string outputDir, int returnNo)
        {
            int result = Kadai.CountOrder(orderDir, outputDir);
            if (returnNo != result)
            {
                Assert.Fail(string.Format("戻り値は{0}であることが期待されましたが{1}が返されました。", returnNo, result));
            }

            string anOutputFilePath = outputDir + "\\" + OUTPUT_FILE_NAME;
            if (File.Exists(anOutputFilePath))
            {
                Assert.Fail("出力ファイルが存在します。");
            }
        }
    }
}
