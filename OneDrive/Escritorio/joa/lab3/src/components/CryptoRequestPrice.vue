<template>
  <div class="price-home container-fluid">
    <div class="row">
      <div v-for="crypto in cryptoList" :key="crypto.code" class="col">
        <div class="crypto-card">
          <h5>{{ crypto.name }}</h5>
          <div class="crypto-info">
            <p v-if="crypto.price">
              Compra: ${{ formatNumber(crypto.price.totalAsk) }}
            </p>
            <p v-if="crypto.price">
              Venta: ${{ formatNumber(crypto.price.totalBid) }}
            </p>
            <button @click="handleBuy(crypto)">Comprar</button>
            <button @click="handleSell(crypto)">Vender</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  computed: {
    ...mapGetters(["getBTCPrice", "getETHPrice", "getUSDTPrice"]),
    cryptoList() {
      return [
        { code: "btc", name: "Bitcoin", price: this.getBTCPrice },
        { code: "eth", name: "Ethereum", price: this.getETHPrice },
        { code: "usdt", name: "USDt", price: this.getUSDTPrice },
      ];
    },
  },
  methods: {
    ...mapActions(["CryptoRequestPrice"]),
    formatNumber(number) {
      if (typeof number === "undefined") {
        return "";
      }
      const numStr = number.toString();
      const parts = numStr.split(".");
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      return parts.join(",");
    },
    handleBuy(crypto) {
      console.log("Comprar", crypto);
    },
    handleSell(crypto) {
      console.log("Vender", crypto);
    },
  },
  created() {
    this.CryptoRequestPrice();
  },
};
</script>

<style scoped>
.crypto-card {
  background-color: #1e3a4c;
  border-radius: 10px;
  padding: 20px;
  margin-bottom: 20px;
  color: #fff;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
.crypto-card h5 {
  font-size: 24px;
  margin-bottom: 10px;
}
.crypto-info p {
  font-size: 18px;
  margin-bottom: 8px;
}
.crypto-info button {
  margin-top: 10px;
  padding: 10px 20px;
  background-color: #007bff;
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
.crypto-info button:hover {
  background-color: #0056b3;
}
</style>
