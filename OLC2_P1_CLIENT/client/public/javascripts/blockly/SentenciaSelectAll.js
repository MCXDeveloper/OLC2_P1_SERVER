Blockly.Blocks['all_block'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldLabelSerializable("*"), "select_all");
      this.setPreviousStatement(true, null);
      this.setColour(160);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['all_block'] = function(block) {
    // TODO: Assemble JavaScript into code variable.
    var code = '*';
    return code;
};