Blockly.Blocks['sentencia_order_by'] = {
    init: function() {
        this.appendStatementInput("order_by")
            .setCheck(null)
            .appendField("ORDER BY");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_order_by'] = function(block) {
    var statements_order_by = Blockly.JavaScript.statementToCode(block, 'order_by').split('%').filter(Boolean);
    // TODO: Assemble JavaScript into code variable.
    var code = 'ORDER BY '+ statements_order_by.join(", ").trim().replace("(", "").replace(")", "") + (block.getNextBlock() === null ? ';\n' : '\n');
    return code;
};