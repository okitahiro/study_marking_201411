using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jp.ktsystem.kadai201411.h_okita;
using System.Collections.Generic;

namespace jp.ktsystem.kadai201411.h_okita_Test
{
    [TestClass]
    public class Lv2Test
    {
        //テストデータフォルダ
        private static readonly string TEST_FOLDER = "..\\..\\..\\TestData\\Lv.2\\";
        //入力受注フォルダ名
        private static readonly string INPUT_ORDER_FOLDER = "input\\order";
        //入力入金フォルダ名
        private static readonly string INPUT_INCOME_FOLDER = "input\\income";
        //入力退避ファイル名
        private static readonly string INPUT_RESERVATION = "input\\reservation\\reservation.dat";

        //出力生産指示フォルダ名
        private static readonly string OUTPUT_PRODUCT_FOLDER = "output\\product";
        //出力バックアップフォルダ名
        private static readonly string OUTPUT_BACKUP_FOLDER = "output\\backup";

        //比較対象生産指示フォルダ名
        private static readonly string EXPECTED_PRODUCT_FOLDER = "expected\\product";
        //比較対象バックアップフォルダ名
        private static readonly string EXPECTED_BACKUP_FOLDER = "expected\\backup";
        //比較対象退避ファイル
        private static readonly string EXPECTED_RESERVATION_FOLDER = "expected\\reservation\\reservation.dat";

        //出力ファイル名
        private static readonly string OUTPUT_FILE_NAME = "productorder.out";
        //退避ファイル名
        private static readonly string RESERVATION_FILE_NAME = "reservation.dat";


        [TestMethod]
        public void Lv2N001()
        {
            string path = TEST_FOLDER + "001\\";
            AssertEquals(path, 0);
        }

        [TestMethod]
        public void Lv2N002()
        {
            string path = TEST_FOLDER + "002\\";
            AssertEquals(path, 0);
        }

        [TestMethod]
        public void Lv2N003()
        {
            string path = TEST_FOLDER + "003\\";
            AssertEquals(path, 5);
        }

        [TestMethod]
        public void Lv2E101()
        {
            string path = TEST_FOLDER + "101\\";
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(null, incomeDir, productDir, backupDir, -1);
        }

        [TestMethod]
        public void Lv2E102()
        {
            string path = TEST_FOLDER + "102\\";
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(string.Empty, incomeDir, productDir, backupDir, -1);
        }

        [TestMethod]
        public void Lv2E103()
        {
            string path = TEST_FOLDER + "103\\";
            //受注情報
            string orderDir = path + "Dummiy";
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -1);
        }

        [TestMethod]
        public void Lv2E104()
        {
            string path = TEST_FOLDER + "104\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E105()
        {
            string path = TEST_FOLDER + "105\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E106()
        {
            string path = TEST_FOLDER + "106\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E107()
        {
            string path = TEST_FOLDER + "107\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E108()
        {
            string path = TEST_FOLDER + "108\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E109()
        {
            string path = TEST_FOLDER + "109\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E110()
        {
            string path = TEST_FOLDER + "110\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -2);
        }

        [TestMethod]
        public void Lv2E111()
        {
            string path = TEST_FOLDER + "111\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, null, productDir, backupDir, -3);
        }

        [TestMethod]
        public void Lv2E112()
        {
            string path = TEST_FOLDER + "112\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, string.Empty, productDir, backupDir, -3);
        }

        [TestMethod]
        public void Lv2E113()
        {
            string path = TEST_FOLDER + "113\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + "Dummiy";
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -3);
        }

        [TestMethod]
        public void Lv2E114()
        {
            string path = TEST_FOLDER + "114\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -4);
        }

        [TestMethod]
        public void Lv2E115()
        {
            string path = TEST_FOLDER + "115\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -4);
        }

        [TestMethod]
        public void Lv2E116()
        {
            string path = TEST_FOLDER + "116\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -4);
        }

        [TestMethod]
        public void Lv2E117()
        {
            string path = TEST_FOLDER + "117\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -4);
        }

        [TestMethod]
        public void Lv2E118()
        {
            string path = TEST_FOLDER + "118\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, null, backupDir, -5);
        }

        [TestMethod]
        public void Lv2E119()
        {
            string path = TEST_FOLDER + "119\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, string.Empty, backupDir, -5);
        }

        [TestMethod]
        public void Lv2E120()
        {
            string path = TEST_FOLDER + "120\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + "Dummiy";
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -5);
        }

        [TestMethod]
        public void Lv2E121()
        {
            string path = TEST_FOLDER + "121\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -6);
        }

        [TestMethod]
        public void Lv2E122()
        {
            string path = TEST_FOLDER + "122\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + OUTPUT_BACKUP_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, backupDir, -7);
        }

        [TestMethod]
        public void Lv2E123()
        {
            string path = TEST_FOLDER + "123\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, null, -9);
        }

        [TestMethod]
        public void Lv2E124()
        {
            string path = TEST_FOLDER + "124\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;

            AssertFail(orderDir, incomeDir, productDir, string.Empty, -9);
        }

        [TestMethod]
        public void Lv2E125()
        {
            string path = TEST_FOLDER + "125\\";
            //受注情報
            string orderDir = path + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = path + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = path + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = path + "Dummiy";

            AssertFail(orderDir, incomeDir, productDir, backupDir, -9);
        }

        /// <summary>
        /// 正常時実行
        /// </summary>
        /// <param name="testFolderDir">テストデータフォルダディレクトリ</param>
        /// <param name="returnNo">戻り値</param>
        private void AssertEquals(string testFolderDir, int returnNo)
        {
            //前提となる退避ファイルがあればコピー
            string inputReservation = testFolderDir + INPUT_RESERVATION;
            if (File.Exists(inputReservation))
            {
                string reservationDir = Kadai.GetReservationFileDir();
                if (!Directory.Exists(reservationDir))
                {
                    Directory.CreateDirectory(reservationDir);
                }
                string reservationPath = reservationDir + "\\" + RESERVATION_FILE_NAME;
                File.Copy(inputReservation, reservationPath, true);
            }
            else
            {
                string reservationDir = Kadai.GetReservationFileDir();
                if (!Directory.Exists(reservationDir))
                {
                    Directory.CreateDirectory(reservationDir);
                }
                string reservationPath = reservationDir + "\\" + RESERVATION_FILE_NAME;
                if (File.Exists(reservationPath))
                {
                    File.Delete(reservationPath);
                }
            }

            //受注情報
            string orderDir = testFolderDir + INPUT_ORDER_FOLDER;
            //入金情報
            string incomeDir = testFolderDir + INPUT_INCOME_FOLDER;
            //生産指示
            string productDir = testFolderDir + OUTPUT_PRODUCT_FOLDER;
            //バックアップ
            string backupDir = testFolderDir + OUTPUT_BACKUP_FOLDER;

            //対象メソッド実行
            int result = Kadai.createProductOrder(orderDir, incomeDir, productDir, backupDir);
            //入力受注ファイルを元に戻す
            string anExBackupFolder = testFolderDir + EXPECTED_BACKUP_FOLDER;
            string[] exBackupFiles = Directory.GetFiles(anExBackupFolder, "*", SearchOption.TopDirectoryOnly);
            foreach (string failPath in exBackupFiles)
            {
                string fileName = Path.GetFileName(failPath);
                //ファイルのコピー
                File.Copy(failPath, orderDir + "\\" + fileName, true);
            }

            if (returnNo != result)
            {
                Assert.Fail(string.Format("戻り値は{0}であることが期待されましたが{1}が返されました。", returnNo, result));
            }

            string anOutputProductFilePath = productDir + "\\" + OUTPUT_FILE_NAME;
            if (!File.Exists(anOutputProductFilePath))
            {
                Assert.Fail("出力ファイルが存在しませんでした。");
            }

            //生産指示ファイル比較
            string anExpextedProductFilePath = testFolderDir + EXPECTED_PRODUCT_FOLDER + "\\" + OUTPUT_FILE_NAME;
            FileStream fs1 = new FileStream(anOutputProductFilePath, FileMode.Open);
            FileStream fs2 = new FileStream(anExpextedProductFilePath, FileMode.Open);
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
                        Assert.Fail("生産指示ファイルの中身が異なります。");
                    }

                    while (byte1 != -1 || byte2 != -1)
                    {
                        byte1 = fs1.ReadByte();
                        byte2 = fs2.ReadByte();
                        if (byte1 != byte2)
                        {
                            Assert.Fail("生産指示ファイルの中身が異なります。");
                        }
                    }
                }
                else
                {
                    Assert.Fail("生産指示ファイルの中身が異なります。");
                }
            }
            finally
            {
                fs1.Close();
                fs2.Close();
            }

            //バックアップファイル比較
            string anBackupFolder = testFolderDir + OUTPUT_BACKUP_FOLDER;
            string[] backupFiles = Directory.GetFiles(anBackupFolder, "*", SearchOption.TopDirectoryOnly);
            if (backupFiles.Length != exBackupFiles.Length)
            {
                Assert.Fail("バックアップファイル数が異なります。");
            }
            for (int i = 0; i < backupFiles.Length; i++)
            {
                FileStream bfs1 = new FileStream(backupFiles[i], FileMode.Open);
                FileStream bfs2 = new FileStream(exBackupFiles[i], FileMode.Open);
                int bbyte1;
                int bbyte2;
                try
                {
                    if (bfs1.Length == bfs2.Length)
                    {
                        bbyte1 = bfs1.ReadByte();
                        bbyte2 = bfs2.ReadByte();
                        if (bbyte1 != bbyte2)
                        {
                            Assert.Fail("出力ファイルの中身が異なります。");
                        }

                        while (bbyte1 != -1 || bbyte2 != -1)
                        {
                            bbyte1 = bfs1.ReadByte();
                            bbyte2 = bfs2.ReadByte();
                            if (bbyte1 != bbyte2)
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
                    bfs1.Close();
                    bfs2.Close();
                }
            }

            //退避ファイル比較
            string exReservation = testFolderDir + EXPECTED_RESERVATION_FOLDER;
            string reservation = Kadai.GetReservationFileDir() + "\\" + RESERVATION_FILE_NAME;

            if (!File.Exists(exReservation))
            {
                if (!File.Exists(reservation))
                {
                    return;
                }
            }

            fs1 = new FileStream(reservation, FileMode.Open);
            fs2 = new FileStream(exReservation, FileMode.Open);
            try
            {
                if (fs1.Length == fs2.Length)
                {
                    byte1 = fs1.ReadByte();
                    byte2 = fs2.ReadByte();
                    if (byte1 != byte2)
                    {
                        Assert.Fail("退避ファイルの中身が異なります。");
                    }

                    while (byte1 != -1 || byte2 != -1)
                    {
                        byte1 = fs1.ReadByte();
                        byte2 = fs2.ReadByte();
                        if (byte1 != byte2)
                        {
                            Assert.Fail("退避ファイルの中身が異なります。");
                        }
                    }
                }
                else
                {
                    Assert.Fail("退避ファイルの中身が異なります。");
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
        /// <param name="orderDir">受注情報ディレクトリ</param>
        /// <param name="incomeDir">入金情報ディレクトリ</param>
        /// <param name="productDir">製品指示出力ディレクトリ</param>
        /// <param name="backupDir">バックアップ出力ディレクトリ</param>
        /// <param name="errorCode">エラーコード</param>
        private void AssertFail(string orderDir, string incomeDir, string productDir, string backupDir, int returnNo)
        {
            int result = Kadai.createProductOrder(orderDir, incomeDir, productDir, backupDir);
            if (returnNo != result)
            {
                Assert.Fail(string.Format("戻り値は{0}であることが期待されましたが{1}が返されました。", returnNo, result));
            }
        }
    }
}
