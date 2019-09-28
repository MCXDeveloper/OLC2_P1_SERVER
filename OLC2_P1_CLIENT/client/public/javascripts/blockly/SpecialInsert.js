Blockly.Blocks['sentencia_insert_especial'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("INSERT INTO")
          .appendField(new Blockly.FieldTextInput("Table"), "nombre_tabla");
      this.appendStatementInput("FIELDS")
          .setCheck("String")
          .appendField("FIELDS");
      this.appendStatementInput("VALUES")
          .setCheck("String")
          .appendField("VALUES");
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour(230);
   this.setTooltip("");
   this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_insert_especial'] = function(block) {
    var text_nombre_tabla = block.getFieldValue('nombre_tabla');
    var statements_fields = Blockly.JavaScript.statementToCode(block, 'FIELDS').split('%').filter(Boolean);
    var statements_values = Blockly.JavaScript.statementToCode(block, 'VALUES').split('%').filter(Boolean);
    // TODO: Assemble JavaScript into code variable.
    var code = 'INSERT INTO '+ text_nombre_tabla +' ( '+ statements_fields.join(", ").trim() +' ) VALUES ( '+ statements_values.join(", ").trim() +' );\n';
    return code;
};