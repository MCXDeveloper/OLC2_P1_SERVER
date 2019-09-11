class LUPBuilder {

    static actualUser;

    static BuildLogin(username, password) {

        let message = "";

        message += "[+LOGIN]\n";
        message += "\t[+USER]\n";
        message += "\t\t"+ username +"\n";
        message += "\t[-USER]\n";
        message += "\t[+PASS]\n";
        message += "\t\t"+ password +"\n";
        message += "\t[-PASS]\n";
        message += "[-LOGIN]\n";

        return message;

    }

    static BuildLogout(username) {

        let message = "";

        message += "[+LOGOUT]\n";
        message += "\t[+USER]\n";
        message += "\t\t"+ username +"\n";
        message += "\t[-USER]\n";
        message += "[-LOGOUT]\n";

        return message;

    }

}