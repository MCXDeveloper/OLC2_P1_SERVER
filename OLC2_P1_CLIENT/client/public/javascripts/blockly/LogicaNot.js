Blockly.Blocks['logica_not'] = {
    init: function() {
      this.appendValueInput("campo_not")
          .setCheck(null)
          .appendField("NOT");
      this.appendDummyInput();
      this.setOutput(true, null);
      this.setColour(330);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.Blocks['up_down_logica_not'] = {
    init: function() {
      this.appendValueInput("campo_not")
          .setCheck(null)
          .appendField("NOT");
      this.appendDummyInput();
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(330);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['logica_not'] = function(block) {
    var value_campo_not = Blockly.JavaScript.valueToCode(block, 'campo_not', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = '!(' + value_campo_not + ')';
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['up_down_logica_not'] = function(block) {
    var value_campo_not = Blockly.JavaScript.valueToCode(block, 'campo_not', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = '!(' + value_campo_not + ')';
    return code;
};