# Brick-Breaker
Проектна задача по Визуелно програмирање - Brick Breaker игра

Датум – 5.11.2017

Автор – Дамјан Најдов, 151506


**За играта**

![src0](http://i.pi.gy/kQZ8.png)

Овој проект претставува една имплементација на познатата игра “Breakout” од 1976. Целта на играта е да се уништат сите таканаречени цигли на екранот. Тоа се постигнува со погодување на циглите со топче што се движи праволиниски на екранот. Играчот контролира плоча која се движи хоризонтално доле на екранот и пази топчето да се одбива од плочата и да не излета надвор од екранот. Играчот губи доколку топчето излета надвор од екранот.

![scr1](http://i.pi.gy/xxGb.png)

**Контроли**

Со лев клик на глувчето се притискаат копчињата, а со левата и десната стрелка на тастатурата се контролира плочата. Во зависност од тоа од кој дел од плочата ќе се одбие топчето, тоа ќе летне кон различен агол, правејќи ја играта подинамична и недетерминистичка.

![scr2](http://i.pi.gy/bLYZ.png)

**Структура на проектот/решението**

Во проектот, освен автоматски генерираните класи од Visual Studio имам 3 сопствени класи – Game, Sprite и Utils. Game класата ги содржи методите за ажурирање и цртање на сите објекти/сликички во играта, и самите објекти. На почетокот на апликацијата создавам една инстанца од Game класата и во посебен thread го извршувам run() методот. Овој метод е бесконечен циклус кој ги повикува update() и draw() методите на секои 16.6 милисекунди за да ја ажурира сцената.

Sprite класата содржи информации за секој објект во играта како на пример неговата локација, големина, и прозирност, како и  getters/setters за овие атрибути. Исто така содржи и метод кој проверува дали објектот се поклопува со дадена точка или друг објект (overlaps(Sprite s)) и метод кој го исцртува објектот на даден канвас (draw(Graphics g)). Всушност draw() методот во Game класата само го повикува draw() методот на секој објект во играта посебно, и им го предава канвасот на главниот екран.

Utils класата содржи два метода: 
ChangeImageOpacity(Image originalImage, double opacity) - кој прима слика како аргумент и врака нова слика со сменета проѕирност. Ова е скап метод и избегнувам да го повикувам колку што е можно.
optimizedImage(Bitmap image) - кој прима слика како аргумент и ја врака истата слика со сменет формат на бои. Овој метод го повикувам кога за прв пат ги вчитувам сликите во рам меморија и ги забрзува операциите со сликите.

Причината за субоптималниот перформанс е дека Windows Forms класите не користат хардвеско забрзување и не се идеален framework за игри.
