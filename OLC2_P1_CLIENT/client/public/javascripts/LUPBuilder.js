class LUPBuilder {

    static BuildLogin(username, password) {

        let message = "";

        message += "[+LOGIN]\n";
        message += "    [+USER]\n";
        message += "        "+ username +"\n";
        message += "    [-USER]\n";
        message += "    [+PASS]\n";
        message += "        "+ password +"\n";
        message += "    [-PASS]\n";
        message += "[-LOGIN]\n";

        return message;

    }

    static BuildLogout(username) {

        let message = "";

        message += "[+LOGOUT]\n";
        message += "    [+USER]\n";
        message += "        "+ username +"\n";
        message += "    [-USER]\n";
        message += "[-LOGOUT]\n";

        return message;

    }

    static BuildQueryPackage(username, content) {

        let message = "";

        message += "[+QUERY]\n";
        message += "    [+USER]\n";
        message += "        "+ username +"\n";
        message += "    [-USER]\n";
        message += "    [+DATA]\n";
        message += "        "+ content +"\n";
        message += "    [-DATA]\n";
        message += "[-QUERY]\n";

        return message;

    }

    static BuildStructPackage(username) {

        let message = "";

        message += "[+STRUCT]\n";
        message += "    [+USER]\n";
        message += "        "+ username +"\n";
        message += "    [-USER]\n";
        message += "[-STRUCT]\n";

        return message;

    }

}