Blockly.Blocks['logicas'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["AND","AND"], ["OR","OR"], ["XOR","XOR"]]), "NAME");
      this.appendDummyInput();
      this.appendValueInput("valorDer")
          .setCheck(null);
      this.setInputsInline(true);
      this.setOutput(true, null);
      this.setColour(330);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.Blocks['up_down_logicas'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["AND","AND"], ["OR","OR"], ["XOR","XOR"]]), "drop_down_log");
      this.appendDummyInput();
      this.appendValueInput("valorDer")
          .setCheck(null);
      this.setInputsInline(true);
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(330);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['logicas'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_name = block.getFieldValue('NAME');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_name + value_valorder;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['up_down_logicas'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_drop_down_log = block.getFieldValue('drop_down_log');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_drop_down_log + value_valorder;
    return code;
};