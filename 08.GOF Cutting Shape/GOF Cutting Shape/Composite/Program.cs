var inputText1 = new InputText("name", "value");
var inputText2 = new InputText("name2", "value2");
var label = new LabelText("labelValue");

var form = new Form("formName");
var finalXMLMarkup = form.AddComponent(label)
                         .AddComponent(inputText1)
                         .AddComponent(inputText2)
                         .ConvertToString();

Console.WriteLine(finalXMLMarkup);