language:C#  
Framework:.netcore2.1  
DataBase:nifcloud  
WebAPI Server:Azure  
WebPage Server:Firebase

１．モデルデータ(大量のテキストデータ)を形態素解析後、ニフティクラウドに下記例のようにデータをInsertします。

Id|Key1|Key2|Key3|Key4|Value
----|----|----|----|----|----
1|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|ライト
2|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|ライト|さん
3|__NULL_OF_SENTENCE__|__NULL_OF_SENTENCE__|ライト|さん|の
4|__NULL_OF_SENTENCE__|ライト|さん|の|馴染み
～|～～～～|～～～～|～～～～|～～～～|～～～～
8|か|ww|さすが|ライト|さん
9|ww|さすが|ライト|さん|だ
10|さすが|ライト|さん|だ|__END_OF_SENTENCE__

２．NCMB.csに下記を設定
		string appKey = アプリケーションキー
		string clientKey = クライアントキー
