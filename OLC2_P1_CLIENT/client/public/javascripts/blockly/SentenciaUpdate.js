Blockly.Blocks['sentencia_update'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("UPDATE")
          .appendField(new Blockly.FieldTextInput("Tabla"), "nombre_tabla");
      this.appendStatementInput("NAME")
          .setCheck(null)
          .appendField("SET");
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_update'] = function(block) {
    var text_nombre_tabla = block.getFieldValue('nombre_tabla');
    var statements_name = Blockly.JavaScript.statementToCode(block, 'NAME').split('%').filter(Boolean);
    // TODO: Assemble JavaScript into code variable.
    var code = 'UPDATE '+ text_nombre_tabla +' SET '+ statements_name.join(", ").trim() + (block.getNextBlock() === null ? ';\n' : '\n');
    return code;
};