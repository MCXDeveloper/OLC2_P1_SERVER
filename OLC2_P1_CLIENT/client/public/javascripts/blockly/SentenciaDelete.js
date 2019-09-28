Blockly.Blocks['sentencia_delete'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("DELETE FROM")
          .appendField(new Blockly.FieldTextInput("Tabla"), "nombre_tabla");
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_delete'] = function(block) {
    var text_nombre_tabla = block.getFieldValue('nombre_tabla');
    // TODO: Assemble JavaScript into code variable.
    var code = 'DELETE FROM '+ text_nombre_tabla + (block.getNextBlock() === null ? ';\n' : '\n');
    return code;
};