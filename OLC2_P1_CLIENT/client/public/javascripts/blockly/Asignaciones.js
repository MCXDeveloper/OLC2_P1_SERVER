Blockly.Blocks['asignacion'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["=","="], ["+=","+="], ["-=","-="], ["*=","*="], ["/=","/="]]), "tipo_asignacion");
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

Blockly.Blocks['up_down_asignacion'] = {
    init: function() {
      this.appendValueInput("valorIzq")
          .setCheck(null);
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["=","="], ["+=","+="], ["-=","-="], ["*=","*="], ["/=","/="]]), "tipo_asignacion");
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

Blockly.JavaScript['asignacion'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_tipo_asignacion = block.getFieldValue('tipo_asignacion');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_tipo_asignacion + value_valorder;
    // TODO: Change ORDER_NONE to the correct strength.
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript['up_down_asignacion'] = function(block) {
    var value_valorizq = Blockly.JavaScript.valueToCode(block, 'valorIzq', Blockly.JavaScript.ORDER_ATOMIC);
    var dropdown_tipo_asignacion = block.getFieldValue('tipo_asignacion');
    var value_valorder = Blockly.JavaScript.valueToCode(block, 'valorDer', Blockly.JavaScript.ORDER_ATOMIC);
    // TODO: Assemble JavaScript into code variable.
    var code = value_valorizq + dropdown_tipo_asignacion + value_valorder;
    return code;
};