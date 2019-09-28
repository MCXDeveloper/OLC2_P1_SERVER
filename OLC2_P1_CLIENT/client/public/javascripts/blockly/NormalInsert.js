Blockly.Blocks['sentencia_insert_normal'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("INSERT INTO")
          .appendField(new Blockly.FieldTextInput("Table"), "nombre_tabla");
      this.appendStatementInput("VALUES")
          .setCheck("String")
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("VALUES");
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(290);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_insert_normal'] = function(block) {
    var text_nombre_tabla = block.getFieldValue('nombre_tabla');
    var vals = Blockly.JavaScript.statementToCode(block, 'VALUES').split('%').filter(Boolean);
    // TODO: Assemble JavaScript into code variable.
    var code = 'INSERT INTO '+ text_nombre_tabla +' VALUES ( '+ vals.join(", ").trim() +' );\n';
    return code;
};