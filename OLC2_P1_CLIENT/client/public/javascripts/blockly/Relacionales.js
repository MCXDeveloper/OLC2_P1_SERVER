Blockly.Blocks['relacionales'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["==","=="], ["! =","!="], ["<","<"], [">",">"], ["<=","<="], [">=",">="]]), "dropdown_booleanos");
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

Blockly.Blocks['up_down_relacionales'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["==","=="], ["!=","!="], ["<","<"], [">",">"], ["<=","<="], [">=",">="]]), "dropdown_bools");
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

Blockly.JavaScript['relacionales'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_dropdown_booleanos = block.getFieldValue('dropdown_booleanos');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_dropdown_booleanos + value_valorder;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['up_down_relacionales'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_dropdown_bools = block.getFieldValue('dropdown_bools');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_dropdown_bools + value_valorder;
    return code;
};