# New Translator 
##### for Visual Studio
A super versatile way to translate, edit and replace any selected text directly from the Visual Studio editor with [Bing Translator](https://www.bing.com/translator) and [Google Translator](https://translate.google.com).

![In action](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_full_context.png)


_This extension is based on the Bender Bending Rodriguez [Translator project](http://vstranslator.codeplex.com) with some improvements. See it [here](https://visualstudiogallery.msdn.microsoft.com/f2321406-c5bb-42b7-9660-dfacd313eeed)._


Well as the original extension you can translate selected text using the context menu or assign a keyboard shortcut than translations are shown in popups below selected text.

####Improvements

- Everything about keyboard.
 - You can use the arrows keys to select the translation (up and down), open and close the item options menu (left and right).
 - Close any party with "Esc".
 - Copy to clipboard the selected translation on list with "Ctrl + C" or copy all on item menu.
 - Assign a keyboard shortcut to command on "Option > Environment > Keyboard" and find to "EditorContextMenus.CodeWindow.TranslateSelection".
 - Finally replace the text with the translation using "Enter" ou "Spacebar".
 
 	_**Important to ReSharper users:** Use the "Spacebar" as the "Enter" does not behave properly._

- Get **translations of both services at the same time** in results (configurable). So you can choose what best fits.
- Translations are kept in memory cache, this allowing instant translation of repetitive texts.
- Can edit and replace the translation. Will be available in the list the next time you translate the same text.
- A more integrated experience. The style match with theme chosen for Visual Studio.


**Please obtain and configure on "Options > New Translator" your own Bing Translator credential to avoid cutting the service. [See how to here](http://blogs.msdn.com/b/translation/p/gettingstarted1.aspx).**

Visual Studio themes match:

![With dark theme](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_open_dark.png)
![With blue theme](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_open_blue.png)
![With light theme](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_open_light.png)

Visual Studio Options:

![Options](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_options_config.png)

Assign Visual Studio keyboard:

![Options](https://raw.github.com/julianpaulozzi/VSNewTranslator/master/res/prints/img_keyboard_mapping.png)


****

#####Julian Paulozzi - Paulozzi&Co 2015.