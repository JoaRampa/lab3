<template>
  <div>
    <img src="@/assets/arrow.png" alt="image" style="width: 100px" />
    <b style="font-size: 30px"> Resumen de Inversiones</b>
    <img src="@/assets/arrow.png" alt="image" style="width: 100px" />
  </div>
  <div class="component">
    <div class="investments">
      <div class="infoProfit">
        <h2><img style="width: 30px" src="@/assets/profit.png" /></h2>
        <h2><img style="width: 30px" src="@/assets/buys.png" /></h2>
        <h2><img style="width: 30px" src="@/assets/sale.png" /></h2>
        <h2><img style="width: 30px" src="@/assets/arrow.png" /></h2>
      </div>
      <div class="infoProfit">
        <p>Poseciones:</p>
        <p>Gastos por compras</p>
        <p>Retornos por ventas</p>
        <p>Total Profit:</p>
      </div>
      <div class="infoProfit">
        <p>$ {{ totalCash }}</p>
        <p>$ {{ calculateBuys() }}</p>
        <p>$ {{ calculateSale() }}</p>
        <p>$ {{ calculateTotalProfit() }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  data() {
    return {
      transactions: [],
      money: 0,
    };
  },
  computed: {
    ...mapGetters(["userId"]),
    ...mapGetters("transactions", ["getWallet"]),
    ...mapGetters(["getBTCPrice", "getETHPrice", "getUSDTPrice"]),
    totalCash() {
      let total = 0;
      for (let cryptoCode in this.getWallet) {
        total += this.calculateCash(this.getWallet[cryptoCode], cryptoCode);
      }
      return total;
    },
  },
  methods: {
    ...mapActions("transactions", ["getState"]),
    ...mapActions("transactions", ["getHistory"]),
    calculateBuys() {
      let buys = 0;
      for (let index = 0; index < this.transactions.length; index++) {
        let transaction = this.transactions[index];

        if (transaction.action === "purchase") {
          buys -= transaction.money;
        }
      }
      return buys;
    },
    calculateSale() {
      let sales = 0;
      for (let index = 0; index < this.transactions.length; index++) {
        let transaction = this.transactions[index];

        if (transaction.action === "sale") {
          sales += transaction.money;
        }
      }
      return sales;
    },
    calculateCash(amount, cryptoCode) {
      const code = cryptoCode.toUpperCase();
      const cryptoGetter = `get${code}Price`;
      const cryptoPrice = this[cryptoGetter];
      if (cryptoPrice) {
        const cash = parseFloat(amount * cryptoPrice.totalAsk);
        return cash;
      } else {
        console.error(`Getter ${cryptoGetter} no encontrado`);
        return 0;
      }
    },
    calculateTotalProfit() {
      let totalBuys = this.calculateBuys();
      let totalSales = this.calculateSale();
      let totalCashValue = this.totalCash;

      let totalProfit = totalBuys + totalSales + totalCashValue;
      return totalProfit.toFixed(2);
    },
    async fetchData() {
      try {
        await this.getState(this.userId);
      } catch (error) {
        console.error("Error:", error);
      }
    },
  },
  async created() {
    try {
      this.transactions = await this.getHistory(this.userId);
      this.fetchData();
    } catch (error) {
      console.error("Error al obtener el historial:", error);
    }
  },
};
</script>

<style scoped>
.component {
  display: flex;
  justify-content: center;
  align-items: center;
}

.investments {
  display: flex;
  justify-content: space-between;
  text-align: left;
  border: 1px solid #35314a;
  border-radius: 15px;
}

.infoProfit {
  padding: 30px;
}

.infoProfit p {
  margin: 15px 0px 20px;
}
</style>
