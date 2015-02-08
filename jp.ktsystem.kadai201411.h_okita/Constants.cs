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
        public enum ErrorCode
        {
            /// <summary>受注情報ファイル入力エラー</summary>
            INPUT_ERROR_OF_ORDER_FILE = -1,
            /// <summary>受注情報ファイルフォーマットエラー</summary>
            FORMAT_ERROR_OF_ORDER_FILE = -2,
            /// <summary>入金情報ファイル入力エラー</summary>
            INPUT_ERROR_OF_INCOME_FILE = -3,
            /// <summary>入金情報ファイルフォーマットエラー</summary>
            FORMAT_ERROR_OF_INCOME_FILE = -4,
            /// <summary>生産指示ファイル出力エラー</summary>
            OUTPUT_ERROR_OF_PRODUCT_ORDER_FILE = -5,
            /// <summary>退避ファイル入力エラー</summary>
            INPUT_ERROR_OF_RESERVATION_FILE = -6,
            /// <summary>退避ファイル出力エラー</summary>
            OUTPUT_ERROR_OF_RESERVATION_FILE = -7,
            /// <summary>問1のファイル出力エラー</summary>
            OUTPUT_ERROR_OF_FIRST_QUESTION = -8,
            /// <summary>バックアップ失敗</summary>
            ERROR_OF_BACKUP = -9,
            /// <summary>計算エラー(オーバーフロー等)</summary>
            ERROR_OF_CALCULATION = -10
        }
    }
}
