Blockly.Blocks['sentencia_limit'] = {
    init: function() {
      this.appendValueInput("NAME")
          .setCheck(null)
          .appendField("LIMIT");
      this.setPreviousStatement(true, null);
      this.setColour(290);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_limit'] = function(block) {
    var value_name = Blockly.JavaScript.valueToCode(block, 'NAME', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = 'LIMIT '+ value_name +';\n';
    return code;
};