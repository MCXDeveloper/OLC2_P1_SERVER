Blockly.Blocks['expresion'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldTextInput("Expresion"), "valor_expresion");
      this.setOutput(true, null);
      this.setColour(120);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};
  
  Blockly.Blocks['entero'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldNumber(0), "valor_entero");
      this.setOutput(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};
  
  Blockly.Blocks['decimal'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldNumber(0, -Infinity, Infinity, 2), "valor_decimal");
      this.setOutput(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};
  
  Blockly.Blocks['booleano'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["TRUE","TRUE"], ["FALSE","FALSE"]]), "drop_bool");
      this.setOutput(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};
  
  Blockly.Blocks['expresion_otro'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldTextInput("Expresion"), "valor_expresion");
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(60);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['expresion'] = function(block) {
    var text_valor_expresion = block.getFieldValue('valor_expresion');
    // TODO: Assemble JavaScript into code variable.
    var code = text_valor_expresion;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};
  
Blockly.JavaScript['entero'] = function(block) {
    var number_valor_entero = block.getFieldValue('valor_entero');
    // TODO: Assemble JavaScript into code variable.
    var code = number_valor_entero;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};
  
Blockly.JavaScript['decimal'] = function(block) {
    var number_valor_decimal = block.getFieldValue('valor_decimal');
    // TODO: Assemble JavaScript into code variable.
    var code = number_valor_decimal;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};
  
Blockly.JavaScript['booleano'] = function(block) {
    var dropdown_drop_bool = block.getFieldValue('drop_bool');
    // TODO: Assemble JavaScript into code variable.
    var code = dropdown_drop_bool;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};
  
Blockly.JavaScript['expresion_otro'] = function(block) {
    var text_valor_expresion = block.getFieldValue('valor_expresion');
    // TODO: Assemble JavaScript into code variable.
	var code = text_valor_expresion + "%";
    return code;
};