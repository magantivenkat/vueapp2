import Plugin from "@ckeditor/ckeditor5-core/src/plugin";
import Model from "@ckeditor/ckeditor5-ui/src/model";
import Collection from "@ckeditor/ckeditor5-utils/src/collection";
import {
  addListToDropdown,
  createDropdown
} from "@ckeditor/ckeditor5-ui/src/dropdown/utils";
import SplitButtonView from "@ckeditor/ckeditor5-ui/src/dropdown/button/splitbuttonview";
import { grconfig } from "./grconfig";

export default class DataTags extends Plugin {
  init() {
    
    const items = new Collection();

    window.$.ajax({
      type: 'GET',
      url: grconfig.baseProjectUrl + "/datatags/list",
      cache: false,
        success: function (result) {
            for (let i = 0; i < result.length; i++) {
                let category = result[i];
                let categoryId = 'category' + i;   
                              
                for (let j = 0; j < category.tags.length; j++) {
                  let tag = category.tags[j]
                  items.add({
                    type: "button",
                    model: new Model({
                      withText: true,
                      label: category.name + ' - '+ tag.name,
                      Text: tag.dataTag
                    })
                  });
                }
                
            }
        }
    })

    const editor = this.editor;
    editor.ui.componentFactory.add("DataTags", locale => {
      const dropdownView = createDropdown(locale, SplitButtonView);
      dropdownView.buttonView.actionView.set({
        withText: true,
        label: "Data Tags",        
        tooltip: true
      });
      
      addListToDropdown(dropdownView, items);

      
      dropdownView.on('execute', (eventInfo) => {
        const { id, label,Text } = eventInfo.source;                    
            editor.model.change( writer => {
              writer.insertText( Text, editor.model.document.selection.getFirstPosition() );
          } );
    });


      return dropdownView;
    });
  }
}
