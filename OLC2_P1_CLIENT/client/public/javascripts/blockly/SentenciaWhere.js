Blockly.Blocks['sentencia_where'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("WHERE");
      this.appendValueInput("NAME")
          .setCheck("String");
      this.setInputsInline(true);
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(160);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_where'] = function(block) {
    var value_name = Blockly.JavaScript.valueToCode(block, 'NAME', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = 'WHERE '+ value_name + (block.getNextBlock() === null ? ';\n' : '\n');
    return code;
};