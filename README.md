# Mini_Basketball_Game
* Bu projede Object Pooling Pattern, Singleton Pattern, Observer Pattern, Dictinory ve Scriptable Object kullanılmıştır. 
* Levellerde atılan toplar Object Pooling yapısında saklanmaktadır.
* BallPooling, GameManager ve LevelController scriptlerinde Singleton Pattern kullanıldı.
* Top oluşması, topun fırlatılması, skorun değişmesi ve UI'daki top sayısının azaltılması gibi işlemler Events kullanılarak yapıldı.
* Levellere özel top sayısı, potanın konumu gibi bilgiler Dictinory yapısında tutuldu.
* DefaultBall, SpecialBall1 ve SpecialBall2 isimlerinde 3 adet Scriptable Object kullanıldı. Bu objeler sayesinde Pooling'de oluşturulmuş olan topların size, mass and color renkleri levellere göre uygulandı.
