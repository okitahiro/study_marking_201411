using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jp.ktsystem.kadai201411.h_okita
{
    /// <summary>
    /// 共通定数クラス
    /// </summary>
    public static class Constants
    {
        /// <summary>正常終了</summary>
        public static readonly int SUCCESS = 0;
        /// <summary>受注情報ファイル入力エラー</summary>
        public static readonly int INPUT_ERRER_OF_ORDER_FILE = -1;
        /// <summary>受注情報ファイルフォーマットエラー</summary>
        public static readonly int FORMAT_ERRER_OF_ORDER_FILE = -2;
        /// <summary>入金情報ファイル入力エラー</summary>
        public static readonly int INPUT_ERRER_OF_INCOME_FILE = -3;
        /// <summary>入金情報ファイルフォーマットエラー</summary>
        public static readonly int FORMAT_ERRER_OF_INCOME_FILE = -4;
        /// <summary>生産指示ファイル出力エラー</summary>
        public static readonly int OUTPUT_ERRER_OF_PRODUCT_ORDER_FILE = -5;
        /// <summary>退避ファイル入力エラー</summary>
        public static readonly int INPUT_ERRER_OF_RESERVATION_FILE = -6;
        /// <summary>退避ファイル出力エラー</summary>
        public static readonly int OUTPUT_ERRER_OF_RESERVATION_FILE = -7;
        /// <summary>問1のファイル出力エラー</summary>
        public static readonly int OUTPUT_ERRER_OF_FIRST_QUESTION = -8;
        /// <summary>バックアップ失敗</summary>
        public static readonly int ERRER_OF_BACKUP = -9;
        /// <summary>計算エラー(オーバーフロー等)</summary>
        public static readonly int ERRER_OF_CALCULATION = -10;
    }
}
